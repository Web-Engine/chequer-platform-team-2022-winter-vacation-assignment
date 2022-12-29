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

    public bool IsReferencing(QualifiedIdentifier identifier)
    {
        if (_relationIdentifier is null)
            return true;

        if (identifier.Level <= 1)
            return false;

        return _relationIdentifier.Equals(identifier.Last());
    }
}