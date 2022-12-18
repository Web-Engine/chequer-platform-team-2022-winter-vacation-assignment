using CsvLite.Models.Relations;

namespace CsvLite.Sql.Results;

public interface IRelationResult : ISqlActionResult
{
    IRelation Relation { get; }
}