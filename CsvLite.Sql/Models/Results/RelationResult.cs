using CsvLite.Models.Relations;

namespace CsvLite.Sql.Models.Results;

public class RelationResult : IRelationResult
{
    public IRelation Relation { get; }

    public RelationResult(IRelation relation)
    {
        Relation = relation;
    }
}