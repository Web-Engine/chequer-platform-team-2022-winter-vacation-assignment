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

    IRelationContext IRelationNode.Evaluate(IContext rootContext)
    {
        var context = Resolve(rootContext);

        return Evaluate(context);
    }

    protected virtual IRelationContext Resolve(IContext rootContext)
    {
        return RelationNode.Value.Evaluate(rootContext);
    }

    protected virtual IRelationContext Evaluate(IRelationContext context)
    {
        return context;
    }
}