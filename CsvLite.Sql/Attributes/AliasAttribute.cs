using CsvLite.Models.Attributes;
using CsvLite.Models.Domains;

namespace CsvLite.Sql.Attributes;

public class AliasAttribute : IAttribute
{
    public string[] Alias { get; }
    
    public string Name => Alias.Last();

    public IDomain Domain => _attribute.Domain;

    private readonly IAttribute _attribute;

    public AliasAttribute(IAttribute attribute, string[] alias)
    {
        _attribute = attribute;
        Alias = alias;
    }
}
