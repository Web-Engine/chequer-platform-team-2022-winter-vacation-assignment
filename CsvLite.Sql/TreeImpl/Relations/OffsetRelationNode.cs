using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

internal class OffsetRelationNode : BaseInheritRelationNode
{
    private readonly int _offset;

    public OffsetRelationNode(IRelationNode relationNode, int offset) : base(relationNode)
    {
        _offset = offset;
    }

    protected override IRelationContext Evaluate(IRelationContext context)
    {
        var relation = new InheritRelation(
            context,
            records: context.Records.Skip(_offset)
        );

        return new InheritRelationContext(context, relation);
    }
}