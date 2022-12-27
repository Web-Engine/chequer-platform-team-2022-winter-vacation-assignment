using System.Collections;
using CsvLite.Models.Records;
using CsvLite.Models.Values;

namespace CsvLite.Sql.Models.Records;

public class ConcatRecord : IRecord
{
    public int Count => _record1.Count + _record2.Count;

    public IValue this[int index]
    {
        get
        {
            if (index < _record1.Count)
                return _record1[index];

            return _record2[index - _record1.Count];
        }
    }

    private readonly IRecord _record1;
    private readonly IRecord _record2;

    public ConcatRecord(IRecord record1, IRecord record2)
    {
        _record1 = record1;
        _record2 = record2;
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<IValue> GetEnumerator()
    {
        return _record1.Concat(_record2).GetEnumerator();
    }
}