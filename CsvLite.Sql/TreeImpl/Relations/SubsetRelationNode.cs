using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class SubsetRelationNode : BaseInheritRelationNode
{
    public List<NodeValue<IExplicitAttributeReferenceNode>> ReferenceNodes { get; }

    public SubsetRelationNode(
        IRelationNode relationNode,
        IEnumerable<IExplicitAttributeReferenceNode> referenceNodes
    ) : base(relationNode)
    {
        ReferenceNodes = referenceNodes.Select(node => node.ToNodeValue()).ToList();
    }

    protected override IRelationContext Evaluate(IRelationContext context)
    {
        if (context.Relation is not IWritableRelation writableRelation)
            throw new InvalidOperationException("Cannot create subset of non-writable relation");

        var indexes = ReferenceNodes
            .SelectMany(node => node.Value.GetAttributeIndexes(context, out _))
            .Select((innerIndex, outerIndex) => (innerIndex, outerIndex))
            .ToList();

        var relation = new SubsetRelation(writableRelation, indexes);

        return new InheritRelationContext(context, relation);
    }

    public override IEnumerable<IAttribute> EvaluateAttributes(IContext context)
    {
        var attributes = base.EvaluateAttributes(context).ToList();

        foreach (var referenceNode in ReferenceNodes)
        {
            referenceNode.Value.Is
        }
    }

    public override IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        return base.EvaluateRecords(context);
    }
}