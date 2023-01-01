using Antlr4.Runtime.Misc;
using CsvLite.Models.Identifiers;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.TreeImpl.Attributes;
using CsvLite.Sql.TreeImpl.Expressions;
using CsvLite.Sql.TreeImpl.Relations;
using CsvLite.Sql.Utilities;
using static CsvLite.Sql.Parsers.Antlr.AntlrSqlParser;

namespace CsvLite.Sql.Parsers.Visitors;

public static class RelationVisitor
{
    public static IRelationNode VisitStatementSelect(StatementSelectContext context)
    {
        var attributeDefinitionNodes = VisitSelectItemList(context.selectItemList()).ToList();

        IRelationNode relationNode;

        if (context.clauseFrom() is { } clauseFrom)
            relationNode = VisitClauseFrom(clauseFrom);
        else
            relationNode = new EmptyRowRelationNode();

        if (context.clauseWhere() is { } clauseWhereContext)
            relationNode = VisitClauseWhere(relationNode, clauseWhereContext);

        if (context.clauseGroupBy() is { } clauseGroupBy)
            relationNode = VisitClauseGroupBy(relationNode, clauseGroupBy);
        else
        {
            var isImplicitAggregate = attributeDefinitionNodes.Any(definitionNode =>
                definitionNode.ContainsRecursive(
                    node => node is IImplicitAggregateNode,
                    node => node is IExpressionNode
                )
            );

            if (isImplicitAggregate)
                relationNode = new AggregateRelationNode(relationNode);
        }

        if (context.clauseOrderBy() is { } clauseOrderByContext)
            throw new NotSupportedException("ORDER BY clause not supported yet");

        if (context.clauseLimit() is { } clauseLimitContext)
            relationNode = VisitClauseLimit(relationNode, clauseLimitContext);
        
        relationNode = new ProjectRelationNode(
            relationNode,
            attributeDefinitionNodes
        );

        if (context.clauseUnion() is { } clauseUnionContext)
            relationNode = VisitClauseUnion(relationNode, clauseUnionContext);

        return relationNode;
    }

    public static IEnumerable<IAttributeDefinitionNode> VisitSelectItemList(SelectItemListContext context)
    {
        return context.selectItem().Select(VisitSelectItem);
    }

    public static IAttributeDefinitionNode VisitSelectItem(SelectItemContext context)
    {
        return context switch
        {
            SelectItem_referenceAllContext referenceAllContext => VisitSelectItem_referenceAll(referenceAllContext),
            SelectItem_referenceContext referenceAttributeContext => VisitSelectItem_reference(
                referenceAttributeContext),
            SelectItem_expressionContext expressionContext => VisitSelectItem_expression(expressionContext),

            _ => throw new InvalidOperationException("Unknown SelectItemContext")
        };
    }

    private static IAttributeDefinitionNode VisitSelectItem_referenceAll(SelectItem_referenceAllContext context)
    {
        var allReference = ExpressionVisitor.VisitReferenceAllAttribute(context.referenceAllAttribute());

        return new AllAttributeDefinitionNode(allReference);
    }

    private static IAttributeDefinitionNode VisitSelectItem_reference(SelectItem_referenceContext context)
    {
        var reference = context.referenceAttribute();

        var relationIdentifier = reference.relationIdentifier?.ToIdentifier();
        var attributeIdentifier = reference.attributeIdentifier.ToIdentifier();

        var qualifiedIdentifier = QualifiedIdentifier.Create(relationIdentifier, attributeIdentifier);

        var alias = context.alias?.ToIdentifier() ?? attributeIdentifier;

        return new ExpressionAttributeDefinitionNode(
            alias,
            new AttributeReferenceExpressionNode(
                new AttributeReferenceNode(new ExplicitAttributeReference(qualifiedIdentifier))
            )
        );
    }

    private static IAttributeDefinitionNode VisitSelectItem_expression(SelectItem_expressionContext context)
    {
        var expressionNode = ExpressionVisitor.VisitExpression(context.expression());
        var name = context.alias?.ToIdentifier() ??
                   new Identifier(context.Start.InputStream.GetText(new Interval(context.Start.StartIndex,
                       context.Stop.StopIndex)));

        return new ExpressionAttributeDefinitionNode(name, expressionNode);
    }


    public static IRelationNode VisitClauseFrom(ClauseFromContext context)
    {
        return VisitFromItemList(context.fromItemList());
    }

    public static IRelationNode VisitFromItemList(FromItemListContext context)
    {
        return context.fromItem()
            .Select(VisitFromItem)
            .Aggregate((relationNode1, relationNode2) =>
                new CrossJoinRelationNode(relationNode1, relationNode2)
            );
    }

    public static IRelationNode VisitFromItem(FromItemContext context)
    {
        var relationNode = VisitRelation(context.relation());

        if (context.alias is { } aliasContext)
        {
            relationNode = new AliasRelationNode(
                relationNode,
                aliasContext.ToIdentifier()
            );
        }

        if (context.clauseJoin() is { })
            throw new NotSupportedException("Not supported yet");

        return relationNode;
    }

    public static IRelationNode VisitRelation(RelationContext context)
    {
        return context switch
        {
            Relation_referenceContext referenceContext => VisitRelation_reference(referenceContext),
            Relation_selectContext selectContext => VisitRelation_select(selectContext),

            _ => throw new InvalidOperationException("Unknown RelationContext")
        };
    }

    private static IRelationNode VisitRelation_reference(Relation_referenceContext context)
    {
        return VisitReferenceRelation(context.referenceRelation());
    }

    private static IRelationNode VisitRelation_select(Relation_selectContext context)
    {
        return VisitStatementSelect(context.statementSelect());
    }

    public static IRelationNode VisitReferenceRelation(ReferenceRelationContext context)
    {
        return new ReferenceRelationNode(
            context.relationIdentifier.ToIdentifier()
        );
    }

    public static IRelationNode VisitClauseGroupBy(IRelationNode relationNode, ClauseGroupByContext context)
    {
        var references = ExpressionVisitor.VisitReferenceAttributeList(context.referenceAttributeList());

        if (context.clauseHaving() is { } clauseHavingContext)
        {
            throw new NotSupportedException("Not supported yet");
        }

        return new AggregateRelationNode(
            relationNode,
            references
        );
    }

    public static IRelationNode VisitClauseWhere(IRelationNode baseRelationNode, ClauseWhereContext context)
    {
        var conditionExpressionNode = ExpressionVisitor.VisitExpression(context.expression());

        return new ConditionRelationNode(
            baseRelationNode,
            conditionExpressionNode
        );
    }

    public static IRelationNode VisitClauseLimit(IRelationNode baseRelationNode, ClauseLimitContext context)
    {
        if (context.offset is { } contextOffset)
            baseRelationNode = new OffsetRelationNode(baseRelationNode, contextOffset.GetInteger());

        return new LimitRelationNode(
            baseRelationNode,
            context.count.GetInteger()
        );
    }

    public static IRelationNode VisitClauseUnion(IRelationNode baseRelationNode, ClauseUnionContext context)
    {
        var dataRelationNode = VisitStatementSelect(context.statementSelect());

        return new ConcatRelationNode(
            baseRelationNode,
            dataRelationNode
        );
    }

    public static IRelationNode VisitInsertItem(InsertItemContext context)
    {
        var relationNode = VisitRelation(context.relation());

        if (context.attributeList() is not { } attributeListContext)
            return relationNode;

        var attributes = VisitAttributeList(attributeListContext)
            .Select(identifier => new AttributeReferenceNode(
                new ExplicitAttributeReference(new QualifiedIdentifier(identifier))
            ));

        return new SubsetRelationNode(
            relationNode,
            attributes
        );
    }

    public static IEnumerable<Identifier> VisitAttributeList(AttributeListContext context)
    {
        foreach (var attributeItemContext in context.attributeItem())
        {
            yield return VisitAttributeItem(attributeItemContext);
        }
    }

    public static Identifier VisitAttributeItem(AttributeItemContext context)
    {
        return context.identifier().ToIdentifier();
    }

    public static IRelationNode VisitInsertValues(InsertValuesContext context)
    {
        switch (context)
        {
            case InsertValues_selectContext select:
                return VisitInsertValues_select(select);

            case InsertValues_valuesContext values:
                return InsertValues_values(values);

            default:
                throw new InvalidOperationException($"Unknown InsertValuesContext {context.GetType()}");
        }
    }

    private static IRelationNode VisitInsertValues_select(InsertValues_selectContext context)
    {
        return VisitStatementSelect(context.statementSelect());
    }

    private static ValuesRelationNode InsertValues_values(InsertValues_valuesContext context)
    {
        var expressionNodes = context.valueList().Select(ExpressionVisitor.VisitValueList);

        return new ValuesRelationNode(expressionNodes);
    }
}