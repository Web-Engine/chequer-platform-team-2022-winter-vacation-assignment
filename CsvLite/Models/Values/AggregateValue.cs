using System.Collections;

namespace CsvLite.Models.Values;

public sealed class AggregateValue : IValue, IReadOnlyList<IValue>
{
    public int Count => Values.Count;

    public IValue this[int index] => Values[index];

    public IReadOnlyList<IValue> Values { get; }

    public AggregateValue(IReadOnlyList<IValue> values)
    {
        Values = values;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<IValue> GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    public int CompareTo(IValue? other)
    {
        throw new InvalidOperationException("Cannot compare");
    }

    public bool Equals(IValue? other)
    {
        if (other is not ListValue dbList)
            throw new InvalidOperationException("Cannot compare");

        return this.SequenceEqual(dbList);
    }
}