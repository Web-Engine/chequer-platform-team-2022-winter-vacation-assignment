namespace CsvLite.Sql.Models.Results;

public class AppendRecordResult : IAppendRecordResult
{
    public TimeSpan Elapsed { get; }
    public int Count { get; }

    public AppendRecordResult(int count, TimeSpan elapsed)
    {
        Count = count;
        Elapsed = elapsed;
    }
}