using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.RelationContexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

internal class LimitRelationNode : BaseInheritRelationNode
{
    private readonly int _limit;

    public LimitRelationNode(IRelationNode relationNode, int limit) : base(relationNode)
    {
        _limit = limit;
    }

    protected override IRelationContext Evaluate(IRelationContext context)
    {
        var relation = new InheritRelation(
            context,
            records: context.Records.Take(_limit)
        );

        return new InheritRelationContext(context, relation);
    }
}