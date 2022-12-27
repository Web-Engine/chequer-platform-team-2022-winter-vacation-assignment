namespace CsvLite.Models.Values;

public class TupleValue : IValue
{
    public IReadOnlyList<IValue> Values { get; }

    public TupleValue(IReadOnlyList<IValue> values)
    {
        Values = values;
    }

    public int CompareTo(IValue? other)
    {
        throw new NotImplementedException();
    }

    public bool Equals(IValue? other)
    {
        throw new NotImplementedException();
    }
}