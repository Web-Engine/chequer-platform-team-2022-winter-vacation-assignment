using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public abstract class BaseBinaryRelationNode : IRelationNode
{
    public virtual IEnumerable<INodeValue> Children
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

    // IRelationContext IRelationNode.Evaluate(IContext rootContext)
    // {
    //     var context1 = Resolve1(rootContext);
    //     var context2 = Resolve2(rootContext, context1);
    //
    //     var context = Combine(context1, context2);
    //
    //     return Evaluate(context);
    // }

    public abstract IEnumerable<IAttribute> EvaluateAttributes(IContext context);
    public abstract IEnumerable<IRecord> EvaluateRecords(IRelationContext context);

    // protected virtual IRelationContext Resolve1(IContext context)
    // {
    //     return RelationNode1.Value.Evaluate(context);
    // }

    // protected virtual IRelationContext Resolve2(IContext rootContext, IRelationContext context1)
    // {
    //     return RelationNode2.Value.Evaluate(context1);
    // }

    // protected abstract IRelationContext Combine(IRelationContext context1, IRelationContext context2);

    // protected virtual IRelationContext Evaluate(IRelationContext context)
    // {
    //     return context;
    // }
}