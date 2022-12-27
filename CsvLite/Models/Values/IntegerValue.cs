namespace CsvLite.Models.Values;

public sealed record IntegerValue(int Value) : IValue
{
    public int CompareTo(IValue? other)
    {
        if (other is not IntegerValue integer)
            throw new InvalidOperationException("Cannot compare");

        return Value.CompareTo(integer.Value);
    }

    public bool Equals(IValue? other)
    {
        if (other is not IntegerValue integer)
            return false;

        return Value.Equals(integer.Value);
    }
}