using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.RelationContexts;
using CsvLite.Sql.Models.Records;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class AggregateRelationNode : BaseInheritRelationNode
{
    public override IEnumerable<INodeValue> Children
    {
        get
        {
            foreach (var value in base.Children)
            {
                yield return value;
            }

            foreach (var node in ReferenceNodes)
            {
                yield return node;
            }
        }
    }

    public List<NodeValue<IAttributeReferenceNode>> ReferenceNodes { get; }

    public AggregateRelationNode(IRelationNode baseRelationNode) : base(baseRelationNode)
    {
        ReferenceNodes = new List<NodeValue<IAttributeReferenceNode>>();
    }

    public AggregateRelationNode(IRelationNode baseRelationNode,
        IEnumerable<IAttributeReferenceNode> referenceNodes) : base(baseRelationNode)
    {
        ReferenceNodes = referenceNodes
            .Select(reference => reference.ToNodeValue())
            .ToList();
    }

    protected override IRelationContext Evaluate(IRelationContext context)
    {
        var nonAggregateAttributeIndexes = ReferenceNodes
            .SelectMany(reference =>
            {
                var result = reference.Value.GetAttributeIndexes(context, out var found);

                return found == context
                    ? result
                    : Enumerable.Empty<int>();
            })
            .ToHashSet();

        if (ReferenceNodes.Count != 0 && nonAggregateAttributeIndexes.Count == 0)
            return context;

        var relation = new InheritRelation(
            context,
            records: context.Records
                .GroupBy(record =>
                {
                    var recordContext = new RecordContext(context, record);

                    var values = ReferenceNodes.SelectMany(
                        node => node.Value.GetValues(recordContext, out _)
                    );

                    return new TupleValue(values);
                })
                .Select(group =>
                    new AggregateRecord(group, nonAggregateAttributeIndexes)
                )
        );

        return new InheritRelationContext(context, relation);
    }
}