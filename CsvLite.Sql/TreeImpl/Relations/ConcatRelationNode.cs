using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ConcatRelationNode : BaseInheritRelationNode
{
    public override IEnumerable<INodeValue> Children
    {
        get
        {
            foreach (var node in base.Children)
            {
                yield return node;
            }

            yield return ConcatDataRelationNode;
        }
    }

    public NodeValue<IRelationNode> ConcatDataRelationNode { get; }

    public ConcatRelationNode(IRelationNode baseRelationNode, IRelationNode fooNode) : base(baseRelationNode)
    {
        ConcatDataRelationNode = fooNode.ToNodeValue();
    }

    public override IReadOnlyList<IAttribute> EvaluateAttributes(IContext context)
    {
        var attributes1 = base.EvaluateAttributes(context);
        var attributes2 = ConcatDataRelationNode.Value.EvaluateAttributes(context);

        if (attributes1.Count != attributes2.Count)
            throw new Exception("Cannot concat(union) difference attribute size relations");

        return attributes1;
    }

    public override IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        return base.EvaluateRecords(context)
            .Concat(ConcatDataRelationNode.Value.EvaluateRecords(context));
    }

    // protected override IRelationContext Combine(IRelationContext context1, IRelationContext context2)
    // {
    //     if (context1.Attributes.Count != context2.Attributes.Count)
    //         throw new Exception("Cannot concat(union) difference attribute size relations");
    //
    //     var relation = new InheritRelation(
    //         context1.Relation,
    //         records: context1.Records.Concat(context2.Records)
    //     );
    //
    //     return new InheritRelationContext(context1, relation);
    // }
}