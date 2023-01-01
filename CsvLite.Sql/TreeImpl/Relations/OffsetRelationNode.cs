using CsvLite.Models.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

internal class OffsetRelationNode : BaseInheritRelationNode
{
    private readonly int _offset;

    public OffsetRelationNode(IRelationNode relationNode, int offset) : base(relationNode)
    {
        _offset = offset;
    }

    public override IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        return base.EvaluateRecords(context).Skip(_offset);
    }
}