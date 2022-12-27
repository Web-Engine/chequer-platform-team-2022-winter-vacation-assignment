using CsvLite.Models.Relations;

namespace CsvLite.Sql.Models.Results;

public interface IRelationResult : ISqlActionResult
{
    IRelation Relation { get; }
}