namespace CsvLite.Models.Values;

public sealed record BooleanValue(bool Value) : IValue
{
    public int CompareTo(IValue? other)
    {
        if (other is not BooleanValue boolean)
            throw new InvalidOperationException("Cannot compare");

        return Value.CompareTo(boolean.Value);
    }

    public bool Equals(IValue? other)
    {
        if (other is not BooleanValue boolean)
            throw new InvalidOperationException("Cannot compare");

        return Value.Equals(boolean.Value);
    }
}