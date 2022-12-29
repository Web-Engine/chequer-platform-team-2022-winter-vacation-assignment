using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Attributes;

public class DefaultAttribute : IAttribute
{
    public static readonly IAttribute Empty = new DefaultAttribute(Identifier.Empty);
    
    public Identifier Name { get; }

    public DefaultAttribute(Identifier attributeIdentifier)
    {
        Name = attributeIdentifier;
    }
}