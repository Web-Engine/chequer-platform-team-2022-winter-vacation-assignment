using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public abstract class BaseInheritRelationNode : IRelationNode
{
    public virtual IEnumerable<INodeValue> Children
    {
        get { yield return RelationNode; }
    }

    public NodeValue<IRelationNode> RelationNode { get; }

    protected BaseInheritRelationNode(IRelationNode relationNode)
    {
        RelationNode = relationNode.ToNodeValue();
    }

    public virtual IEnumerable<IAttribute> EvaluateAttributes(IContext context)
    {
        return RelationNode.Value.EvaluateAttributes(context);
    }

    public virtual IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        return RelationNode.Value.EvaluateRecords(context);
    }
}