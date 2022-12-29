using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public abstract class BaseBinaryRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children
    {
        get
        {
            yield return RelationNode1;
            yield return RelationNode2;
        }
    }

    public NodeValue<IRelationNode> RelationNode1 { get; }
    public NodeValue<IRelationNode> RelationNode2 { get; }

    protected BaseBinaryRelationNode(IRelationNode relationNode1, IRelationNode relationNode2)
    {
        RelationNode1 = relationNode1.ToNodeValue();
        RelationNode2 = relationNode2.ToNodeValue();
    }

    IRelation IRelationNode.Evaluate(IRootContext context)
    {
        var relation1 = RelationNode1.Value.Evaluate(context);
        var relation2 = RelationNode2.Value.Evaluate(context);

        var context1 = context.CreateRelationContext(relation1);
        var context2 = context.CreateRelationContext(relation2);

        return Evaluate(context1, context2);
    }

    protected abstract IRelation Evaluate(IRelationContext context1, IRelationContext context2);
}