using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

internal class LimitRelationNode : IInheritRelationNode
{
    public IRelationNode? BaseRelationNode { get; }

    private readonly int _limit;

    public LimitRelationNode(IRelationNode? baseRelationNode, int limit)
    {
        BaseRelationNode = baseRelationNode;
        _limit = limit;
    }

    public IRelation Evaluate(IRelationEvaluateContext context)
    {
        return new InheritRelation(
            context.Relation,
            records: context.Relation.Records.Take(_limit)
        );
    }
}