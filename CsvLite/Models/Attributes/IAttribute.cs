using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Attributes;

public interface IAttribute
{
    Identifier Name { get; }
}
