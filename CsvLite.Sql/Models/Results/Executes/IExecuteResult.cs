namespace CsvLite.Sql.Models.Results.Executes;

public interface IExecuteResult
{
    TimeSpan Elapsed { get; }
}