namespace CsvLite.Sql.Models.Results;

public interface IAppendRecordResult : IExecuteResult
{
    int Count { get; }
}