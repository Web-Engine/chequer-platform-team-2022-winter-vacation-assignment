using CsvLite.Models.Values;

namespace CsvLite.Models.Records;

public class DefaultRecord : List<IValue>, IRecord
{
    public DefaultRecord()
    {
    }
    
    public DefaultRecord(IEnumerable<IValue> values) : base(values)
    {
    }
}