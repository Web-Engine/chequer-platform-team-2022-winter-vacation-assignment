using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;

namespace CsvLite.Sql.Models.Attributes;

public class ExplicitAttributeReference : IAttributeReference
{
    private readonly QualifiedIdentifier _identifier;

    public ExplicitAttributeReference(QualifiedIdentifier identifier)
    {
        _identifier = identifier;
    }

    public bool IsReferencing(IAttribute attribute)
    {
        return _identifier.Level switch
        {
            1 => _identifier[0].Equals(attribute.Name),
            2 => _identifier.Equals(attribute.Alias),
            
            _ => throw new InvalidOperationException($"Wrong identifier level {_identifier.Level}")
        };
    }

    public override string ToString()
    {
        return _identifier.ToString();
    }
}