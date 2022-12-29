using CsvLite.Models.Relations;

namespace CsvLite.Sql.Models.Results.Actions;

public interface IRelationActionResult : IActionResult
{
    IRelation Relation { get; }
}