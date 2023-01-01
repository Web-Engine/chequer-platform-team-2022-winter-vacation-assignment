using CsvLite.Models.Values;

namespace CsvLite.Models.Records;

public sealed class DefaultRecord : List<IValue>, IRecord
{
    public static readonly IRecord Empty = new DefaultRecord();

    public DefaultRecord()
    {
    }

    public DefaultRecord(IEnumerable<IValue> values) : base(values)
    {
    }
}