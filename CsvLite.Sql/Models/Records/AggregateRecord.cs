using System.Collections;
using CsvLite.Models.Records;
using CsvLite.Models.Values;

namespace CsvLite.Sql.Models.Records;

public class AggregateRecord : IRecord
{
    public IGrouping<IValue, IRecord> Grouping { get; }

    public IValue Key => Grouping.Key;

    public int Count => Grouping.First().Count;

    public IValue this[int index]
    {
        get
        {
            var isNonAggregateAttribute = _nonAggregateAttributeIndexes.Contains(index);

            return new AggregateValue(
                Grouping.Select(record => record[index]),
                isNonAggregateAttribute
            );
        }
    }

    private readonly HashSet<int> _nonAggregateAttributeIndexes;

    public AggregateRecord(IGrouping<IValue, IRecord> grouping, HashSet<int> nonAggregateAttributeIndexes)
    {
        Grouping = grouping;
        _nonAggregateAttributeIndexes = nonAggregateAttributeIndexes;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<IValue> GetEnumerator()
    {
        return Enumerate().GetEnumerator();
    }

    private IEnumerable<IValue> Enumerate()
    {
        for (var index = 0; index < Count; index++)
            yield return this[index];
    }
}