namespace CsvLite.Sql.Models.Results.Actions;

public interface IAppendRecordActionResult : IActionResult
{
    int Count { get; }
}