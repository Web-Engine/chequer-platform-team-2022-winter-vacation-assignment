using CsvLite.Models.Domains;
using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Attributes;

public interface IAttribute
{
    Identifier Identifier { get; }
    
    IPrimitiveDomain Domain { get; }
}