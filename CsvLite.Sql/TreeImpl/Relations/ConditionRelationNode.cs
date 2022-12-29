using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;
using CsvLite.Utilities;

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

    protected override IRelation Evaluate(IRelationContext context)
    {
        return new InheritRelation(
            context,
            
            recordFilter: record =>
            {
                var recordContext = context.CreateRecordContext(record);

                var condition = ExpressionNode.Evaluate(recordContext).AsBoolean();
                return condition.Value;
            }
        );
    }
}