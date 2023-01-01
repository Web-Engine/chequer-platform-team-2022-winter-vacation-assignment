using CsvLite.Models.Values;

namespace CsvLite.Models.Domains;

public interface IPrimitiveDomain : IDomain
{
    PrimitiveValue Parse(string? data);
}