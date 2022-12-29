namespace CsvLite.Sql.Models.Results.Executes;

public interface IAppendRecordsResult : IExecuteResult
{
    int Count { get; }
}