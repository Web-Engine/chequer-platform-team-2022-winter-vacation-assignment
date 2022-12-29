using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Actions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Actions;

public class AppendRecordActionNode : IAppendRecordActionNode
{
    public IEnumerable<INodeValue> Children
    {
        get
        {
            yield return TargetRelationNode;
            yield return ValueRelationNode;
        }
    }

    IRelationNode IAppendRecordActionNode.TargetRelationNode => TargetRelationNode.Value;
    IRelationNode IAppendRecordActionNode.ValueRelationNode => ValueRelationNode.Value;

    public NodeValue<IRelationNode> TargetRelationNode { get; }
    public NodeValue<IRelationNode> ValueRelationNode { get; }

    public AppendRecordActionNode(IRelationNode targetRelation, IRelationNode valueRelation)
    {
        TargetRelationNode = targetRelation.ToNodeValue();
        ValueRelationNode = valueRelation.ToNodeValue();
    }
}