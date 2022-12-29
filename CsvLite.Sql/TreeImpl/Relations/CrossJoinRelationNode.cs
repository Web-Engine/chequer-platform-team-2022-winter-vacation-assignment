using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.RelationContexts;
using CsvLite.Sql.Models.Records;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class CrossJoinRelationNode : BaseBinaryRelationNode
{
    public CrossJoinRelationNode(
        IRelationNode baseRelationNode1,
        IRelationNode baseRelationNode2
    ) : base(baseRelationNode1, baseRelationNode2)
    {
    }

    protected override IRelationContext Combine(IRelationContext context1, IRelationContext context2)
    {
        var relation = new DefaultRelation(
            context1.Attributes.Concat(context2.Attributes),
            context1.Records.SelectMany(
                _ => context2.Records,
                (record1, record2) => new ConcatRecord(record1, record2)
            )
        );

        return new CombineRelationContext(context1, context2, relation);
    }
}