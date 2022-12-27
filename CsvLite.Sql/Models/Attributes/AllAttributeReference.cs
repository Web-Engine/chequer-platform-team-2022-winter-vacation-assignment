using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;

namespace CsvLite.Sql.Models.Attributes;

public class AllAttributeReference : IAttributeReference
{
    private readonly Identifier? _relationIdentifier;

    public AllAttributeReference(Identifier? relationIdentifier)
    {
        _relationIdentifier = relationIdentifier;
    }

    public bool IsReferencing(IAttribute attribute)
    {
        if (_relationIdentifier is null)
            return true;

        if (attribute.Alias.Level <= 1)
            return false;

        return _relationIdentifier.Equals(attribute.Alias[0]);
    }
}