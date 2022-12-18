using CsvLite.Models.Domains;

namespace CsvLite.Models.Attributes;

public interface IReadOnlyAttribute
{
    string Name { get; }
    
    IDomain Domain { get; }
}
