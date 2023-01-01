using CsvLite.Models.Attributes;

namespace CsvLite.Models.Domains;

public class RelationDomain : IDomain
{
    public IReadOnlyList<IAttribute> Attributes { get; }

    public RelationDomain(IReadOnlyList<IAttribute> attributes)
    {
        Attributes = attributes;
    }
}