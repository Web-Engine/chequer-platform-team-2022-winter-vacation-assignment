using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;

namespace CsvLite.Models.Domains;

public class IntegerDomain : IPrimitiveDomain
{
    public PrimitiveValue Parse(string? data)
    {
        if (string.IsNullOrEmpty(data))
            return NullValue.Null;

        if (!int.TryParse(data, out var integer))
            throw new InvalidOperationException($"IntegerDomain parse failed {data}");

        return new IntegerValue(integer);
    }

    public override bool Equals(object? obj)
    {
        return obj is IntegerDomain;
    }

    public override int GetHashCode()
    {
        return 0x12345678 * 2;
    }
}