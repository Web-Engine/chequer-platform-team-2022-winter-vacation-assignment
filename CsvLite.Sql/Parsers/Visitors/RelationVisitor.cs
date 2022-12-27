using CsvLite.Models.Identifiers;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.TreeImpl.Attributes;
using CsvLite.Sql.TreeImpl.Expressions;
using CsvLite.Sql.TreeImpl.Relations;
using static CsvLite.Sql.Parsers.Antlr.AntlrSqlParser;

namespace CsvLite.Sql.Parsers.Visitors;

public static class RelationVisitor
{
    public static IRelationNode VisitStatementSelect(StatementSelectContext context)
    {
        var attributeDefinitionNodes = VisitSelectItemList(context.selectItemList());

        IRelationNode? relationNode = null;
        
        if (context.clauseFrom() is { } clauseFrom)
            relationNode = VisitClauseFrom(context.clauseFrom());

        if (context.clauseWhere() is { } clauseWhereContext)
            relationNode = VisitClauseWhere(relationNode, clauseWhereContext);

        if (context.clauseGroupBy() is { } clauseGroupBy)
            throw new NotSupportedException("GROUP BY clause not supported yet");

        if (context.clauseOrderBy() is { } clauseOrderByContext)
            throw new NotSupportedException("ORDER BY clause not supported yet");

        if (context.clauseLimit() is { } clauseLimitContext)
            relationNode = VisitClauseLimit(relationNode, clauseLimitContext);

        return new ProjectRelationNode(
            relationNode,
            attributeDefinitionNodes.ToList()
        );
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
            SelectItem_expressionContext expressionContext => VisitSelectItem_expression(expressionContext),

            _ => throw new InvalidOperationException("Unknown SelectItemContext")
        };
    }

    private static IAttributeDefinitionNode VisitSelectItem_referenceAll(SelectItem_referenceAllContext context)
    {
        var allReference = ExpressionVisitor.VisitReferenceAllAttribute(context.referenceAllAttribute());

        return new AllAttributeDefinitionNode(
            new AllAttributeReferenceExpressionNode(allReference)
        );
    }

    private static IAttributeDefinitionNode VisitSelectItem_expression(SelectItem_expressionContext context)
    {
        var expressionNode = ExpressionVisitor.VisitExpression(context.expression());
        var name = context.alias?.ToIdentifier() ?? new Identifier(context.expression().GetText());

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

    public static IRelationNode VisitClauseWhere(IRelationNode? baseRelationNode, ClauseWhereContext context)
    {
        var conditionExpressionNode = ExpressionVisitor.VisitExpression(context.expression());

        return new ConditionRelationNode(
            baseRelationNode,
            conditionExpressionNode
        );
    }

    public static IRelationNode VisitClauseLimit(IRelationNode? baseRelationNode, ClauseLimitContext context)
    {
        if (context.offset is { } contextOffset)
            baseRelationNode = new OffsetRelationNode(baseRelationNode, contextOffset.GetInteger());

        return new LimitRelationNode(
            baseRelationNode,
            context.count.GetInteger()
        );
    }
}