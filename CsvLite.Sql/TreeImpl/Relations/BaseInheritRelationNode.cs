using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
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

    IRelation IRelationNode.Evaluate(IRootContext context)
    {
        var relation = RelationNode.Value.Evaluate(context);

        var relationContext = context.CreateRelationContext(relation);
        
        return Evaluate(relationContext);
    }

    protected abstract IRelation Evaluate(IRelationContext context);
}