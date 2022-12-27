namespace CsvLite.Models.Values;

public sealed record StringValue(string Value) : IValue
{
    public int CompareTo(IValue? other)
    {
        if (other is not StringValue str)
            throw new InvalidOperationException("Cannot compare");

        return string.Compare(Value, str.Value, StringComparison.Ordinal);
    }

    public bool Equals(IValue? other)
    {
        if (other is not StringValue str)
            throw new InvalidOperationException("Cannot compare");

        return Value.Equals(str.Value);
    }
}
