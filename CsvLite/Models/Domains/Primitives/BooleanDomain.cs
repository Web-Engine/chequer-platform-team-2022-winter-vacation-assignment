using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;

namespace CsvLite.Models.Domains;

public class BooleanDomain : IPrimitiveDomain
{
    public PrimitiveValue Parse(string? data)
    {
        if (string.IsNullOrEmpty(data))
            return NullValue.Null;

        return data.ToUpperInvariant() switch
        {
            "TRUE" => BooleanValue.True,
            "FALSE" => BooleanValue.False,
            
            _ => throw new InvalidOperationException($"BooleanDomain parse failed {data}")
        };
    }

    public override bool Equals(object? obj)
    {
        return obj is BooleanDomain;
    }

    public override int GetHashCode()
    {
        return 0x12345678 * 1;
    }
}