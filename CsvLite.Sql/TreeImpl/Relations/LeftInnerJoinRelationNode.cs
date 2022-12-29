using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Models.Records;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;
using CsvLite.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class LeftInnerJoinRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children
    {
        get
        {
            yield return RelationNode1;
            yield return RelationNode2;
            yield return ExpressionNode;
        }
    }

    public NodeValue<IRelationNode> RelationNode1 { get; }

    public NodeValue<IRelationNode> RelationNode2 { get; }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    public LeftInnerJoinRelationNode(IRelationNode relationNode1, IRelationNode relationNode2,
        IExpressionNode expressionNode)
    {
        RelationNode1 = relationNode1.ToNodeValue();
        RelationNode2 = relationNode2.ToNodeValue();
        ExpressionNode = expressionNode.ToNodeValue();
    }

    public IRelation Evaluate(IRootContext context)
    {
        var relation1 = RelationNode1.Value.Evaluate(context);
        var relation2 = RelationNode2.Value.Evaluate(context);

        var relation1Context = context.CreateRelationContext(relation1);

        return new DefaultRelation(
            new ConcatAttributeList(relation1.Attributes, relation2.Attributes),
            relation1.Records.SelectMany(record1 =>
            {
                var record1Context = relation1Context.CreateRecordContext(record1);
                var relation2Context = record1Context.CreateRelationContext(relation2);

                return relation2.Records.Where(record2 =>
                    {
                        var record2Context = relation2Context.CreateRecordContext(record2);

                        return ExpressionNode.Value.Evaluate(record2Context).AsBoolean().Value;
                    })
                    .Select(record2 => new ConcatRecord(record1, record2));
            })
        );
    }
}