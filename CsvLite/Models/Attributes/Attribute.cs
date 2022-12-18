using CsvLite.Models.Domains;

namespace CsvLite.Models.Attributes;

public class Attribute : IAttribute
{
    public string Name { get; set; }
    
    public IDomain Domain { get; set; }
}
