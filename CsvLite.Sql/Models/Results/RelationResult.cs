using System.Diagnostics;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Models.Results;

public class RelationResult : IRelationResult
{
    public IRelation Relation { get; }
    public TimeSpan Elapsed { get; }

    public RelationResult(IRelation relation, TimeSpan elapsed)
    {
        Relation = relation;
        Elapsed = elapsed;
    }
}