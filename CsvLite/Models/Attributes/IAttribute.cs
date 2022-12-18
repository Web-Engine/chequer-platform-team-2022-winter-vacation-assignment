using CsvLite.Models.Domains;

namespace CsvLite.Models.Attributes;

public interface IAttribute : IReadOnlyAttribute
{
    string IReadOnlyAttribute.Name => Name;

    IDomain IReadOnlyAttribute.Domain => Domain;
    
    new string Name { get; set; }
    
    new IDomain Domain { get; set; }
}
