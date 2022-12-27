namespace CsvLite.Models.Values;

public sealed record NullValue : IValue
{
    public int CompareTo(IValue? other)
    {
        if (other is not NullValue)
            throw new InvalidOperationException("Cannot compare");

        return 0;
    }

    public bool Equals(IValue? other)
    {
        if (other is not NullValue)
            throw new InvalidOperationException("Cannot compare");

        return true;
    }
};
