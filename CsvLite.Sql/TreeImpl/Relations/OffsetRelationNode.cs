using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

internal class OffsetRelationNode : IInheritRelationNode
{
    public IRelationNode? BaseRelationNode { get; }
    
    private readonly int _offset;

    public OffsetRelationNode(IRelationNode? baseRelationNode, int offset)
    {
        BaseRelationNode = baseRelationNode;
        _offset = offset;
    }

    public IRelation Evaluate(IRelationEvaluateContext context)
    {
        return new InheritRelation(
            context.Relation,
            records: context.Relation.Records.Skip(_offset)
        );
    }
}
