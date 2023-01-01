using CsvLite.Models.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

internal class LimitRelationNode : BaseInheritRelationNode
{
    private readonly int _limit;

    public LimitRelationNode(IRelationNode relationNode, int limit) : base(relationNode)
    {
        _limit = limit;
    }

    public override IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        return base.EvaluateRecords(context).Take(_limit);
    }
}