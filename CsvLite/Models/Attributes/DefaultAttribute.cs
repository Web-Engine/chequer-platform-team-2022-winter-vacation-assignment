using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Attributes;

public class DefaultAttribute : IAttribute
{
    public QualifiedIdentifier Alias { get; }

    public Identifier Name { get; }

    public DefaultAttribute(Identifier relationIdentifier, Identifier attributeIdentifier)
    {
        Alias = new QualifiedIdentifier(relationIdentifier, attributeIdentifier);
        Name = attributeIdentifier;
    }
}