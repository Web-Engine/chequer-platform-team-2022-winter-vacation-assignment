using CsvLite.Models.Relations;

namespace CsvLite.Sql.Models.Results.Executes;

public interface IRelationResult : IExecuteResult
{
    IRelation Relation { get; }
}