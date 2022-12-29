using CsvLite.Models.Relations;

namespace CsvLite.Sql.Models.Results;

public interface IRelationResult : IExecuteResult
{
    IRelation Relation { get; }
}