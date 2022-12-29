using CsvLite.Models.Identifiers;

namespace CsvLite.Sql.Models.Attributes;

public interface IAttributeReference
{
    bool IsReferencing(QualifiedIdentifier identifier);
}