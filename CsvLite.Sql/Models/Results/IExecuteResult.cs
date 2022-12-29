namespace CsvLite.Sql.Models.Results;

public interface IExecuteResult
{
    TimeSpan Elapsed { get; }
}