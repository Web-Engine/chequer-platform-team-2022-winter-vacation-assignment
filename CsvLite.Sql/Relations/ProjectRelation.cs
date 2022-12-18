using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Relations;

public class ProjectRelation : IRelation
{
    public IEnumerable<IAttribute> Attributes { get; }
    
    public IEnumerable<ITuple> Tuples { get; }   
}