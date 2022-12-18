using CsvLite.Models.Attributes;
using CsvLite.Models.Tuples;

namespace CsvLite.Models.Relations;

public interface IRelation
{
    IEnumerable<IAttribute> Attributes { get; }
    
    IEnumerable<ITuple> Tuples { get; }
}
