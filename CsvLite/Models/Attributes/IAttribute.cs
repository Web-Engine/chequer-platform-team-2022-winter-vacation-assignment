using CsvLite.Models.Domains;

namespace CsvLite.Models.Attributes;

public interface IAttribute
{
    string[] Alias { get; }
    
    string Name { get; }
    
    IDomain Domain { get; }
}
