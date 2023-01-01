using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;

namespace CsvLite.Models.Domains;

public sealed class StringDomain : IPrimitiveDomain
{
    public PrimitiveValue Parse(string? data)
    {
        if (string.IsNullOrEmpty(data))
            return NullValue.Null;

        return new StringValue(data);
    }

    public override bool Equals(object? obj)
    {
        return obj is StringDomain;
    }

    public override int GetHashCode()
    {
        return 0x12345678 * 3;
    }
}
