using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Attributes;

public interface IAttribute
{
    QualifiedIdentifier Alias { get; }
    
    Identifier Name { get; }
}
