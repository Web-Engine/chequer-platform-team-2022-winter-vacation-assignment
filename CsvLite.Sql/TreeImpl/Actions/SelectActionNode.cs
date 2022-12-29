using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Actions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Actions;

public class SelectActionNode : IRelationActionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return RelationNode; }
    }

    IRelationNode IRelationActionNode.RelationNode => RelationNode.Value;

    public NodeValue<IRelationNode> RelationNode { get; }

    public SelectActionNode(IRelationNode relationNode)
    {
        RelationNode = relationNode.ToNodeValue();
    }
}