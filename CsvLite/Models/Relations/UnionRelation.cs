using System.Dynamic;
using CsvLite.Models.Attributes;
using CsvLite.Models.Tuples;

namespace CsvLite.Models.Relations;

public class UnionRelation : IRelation
{
    private readonly IRelation _previous;
    private readonly IRelation _next;
    
    public IList<IAttribute> Attributes => _previous.Attributes;

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
        _previous = previous;
        _next = next;
    }
}