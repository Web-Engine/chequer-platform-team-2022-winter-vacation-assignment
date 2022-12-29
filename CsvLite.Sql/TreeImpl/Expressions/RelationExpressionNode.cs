using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class RelationExpressionNode : IExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return RelationNode; }
    }

    public NodeValue<IRelationNode> RelationNode { get; }

    public RelationExpressionNode(IRelationNode relationNode)
    {
        RelationNode = relationNode.ToNodeValue();
    }

    public IValue Evaluate(IRecordContext context)
    {
        var relation = RelationNode.Evaluate(context);

        return new RelationValue(relation);
    }
}