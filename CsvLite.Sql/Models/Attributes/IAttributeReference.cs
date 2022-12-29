using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Attributes;

public interface IAttributeReference
{
    bool IsReferencing(QualifiedIdentifier identifier);
}