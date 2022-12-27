using Antlr4.Runtime.Misc;
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
        switch (_identifier.Level)
        {
            case 1:
                return _identifier[0].Equals(attribute.Name);
            
            case 2:
                return _identifier.Equals(attribute.Alias);

            default:
                throw new InvalidOperationException($"Wrong identifier level {_identifier.Level}");
        }
    }

    public override string ToString()
    {
        return _identifier.ToString();
    }
}