using CsvLite.Models.Domains;
using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Attributes;

public class DefaultAttribute : IAttribute
{
    public Identifier Identifier { get; }
    
    public IPrimitiveDomain Domain { get; }

    public DefaultAttribute(Identifier identifier, IPrimitiveDomain domain)
    {
        Identifier = identifier;
        Domain = domain;
    }
}