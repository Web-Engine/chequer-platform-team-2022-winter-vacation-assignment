using System.Dynamic;
using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Relations;

public class UnionRelation : IRelation
{
    private readonly IRelation _previous;
    private readonly IRelation _next;
    
    public IEnumerable<IAttribute> Attributes => _previous.Attributes;

    public IEnumerable<ITuple> Tuples
    {
        get
        {
            foreach (var previousTuple in _previous.Tuples) yield return previousTuple;
            foreach (var nextTuple in _next.Tuples) yield return nextTuple;
        }
    }

    public UnionRelation(IRelation previous, IRelation next)
    {
        if (previous.Attributes.Count() != next.Attributes.Count())
            throw new Exception("Cannot union difference attribute size relations");
        
        _previous = previous;
        _next = next;
    }
}