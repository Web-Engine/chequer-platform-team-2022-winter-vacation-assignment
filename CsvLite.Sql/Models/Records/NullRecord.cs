using CsvLite.Models.Records;
using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;

namespace CsvLite.Sql.Models.Records;

public class NullRecord : List<IValue>, IRecord
{
    public NullRecord(int count) : base(
        Enumerable.Range(0, count).Select(x => NullValue.Null)
    )
    {
    }
}