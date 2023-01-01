using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Models.Records;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;
using CsvLite.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class LeftInnerJoinRelationNode : BaseBinaryRelationNode
{
    public override IEnumerable<INodeValue> Children
    {
        get
        {
            foreach (var node in base.Children)
                yield return node;

            yield return ExpressionNode;
        }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    public LeftInnerJoinRelationNode(
        IRelationNode relationNode1,
        IRelationNode relationNode2,
        IExpressionNode expressionNode
    ) : base(relationNode1, relationNode2)
    {
        ExpressionNode = expressionNode.ToNodeValue();
    }

    protected override IRelationContext Combine(IRelationContext context1, IRelationContext context2)
    {
        var attributes = context1.Attributes.Concat(context2.Attributes);

        var records = context1.Records.SelectMany(record1 =>
        {
            var record1Context = new RecordContext(context1, record1);
            var nestedContext = new NestedRelationContext(record1Context, context2);

            return context2.Records.Where(record2 =>
                {
                    var record2Context = new RecordContext(nestedContext, record2);

                    return ExpressionNode.Value.EvaluateValue(record2Context).AsBoolean().Value;
                })
                .Select(record2 => new ConcatRecord(record1, record2));
        });

        var relation = new DefaultRelation(attributes, records);

        return new CombineRelationContext(context1, context2, relation);
    }
}