using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ConditionRelationNode : BaseInheritRelationNode
{
    public override IEnumerable<INodeValue> Children
    {
        get
        {
            foreach (var node in base.Children)
            {
                yield return node;
            }

            yield return ExpressionNode;
        }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; set; }

    public ConditionRelationNode(IRelationNode relationNode, IExpressionNode expressionNode) : base(relationNode)
    {
        ExpressionNode = expressionNode.ToNodeValue();
    }

    public override IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        return base.EvaluateRecords(context)
            .Where(record =>
            {
                var recordContext = new RecordContext(context, record);

                return ExpressionNode.Value.EvaluateValue(recordContext).AsBoolean().Value;
            });
    }
}