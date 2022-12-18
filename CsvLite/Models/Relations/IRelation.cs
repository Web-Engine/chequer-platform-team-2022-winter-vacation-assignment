using CsvLite.Models.Attributes;
using CsvLite.Models.Tuples;

namespace CsvLite.Models.Relations;

public interface IRelation : IReadOnlyRelation
{
    IEnumerable<IReadOnlyAttribute> IReadOnlyRelation.Attributes => Attributes;
    
    new IList<IAttribute> Attributes { get; }
    
    IEnumerable<ITuple> Tuples { get; }
}
