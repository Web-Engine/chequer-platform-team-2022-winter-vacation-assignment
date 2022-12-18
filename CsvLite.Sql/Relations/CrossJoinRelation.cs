using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;
using CsvLite.Sql.Tuples;

namespace CsvLite.Sql.Relations;

public class CrossJoinRelation : IRelation
{
    public IEnumerable<IAttribute> Attributes
    {
        get
        {
            foreach (var attribute in _baseRelation.Attributes)
            {
                yield return attribute;
            }
            
            foreach (var attribute in _joinRelation.Attributes)
            {
                yield return attribute;
            }
        }
    }

    public IEnumerable<ITuple> Tuples
    {
        get
        {
            foreach (var baseTuple in _baseRelation.Tuples)
            {
                foreach (var joinTuple in _joinRelation.Tuples)
                {
                    yield return new JoinTuple(baseTuple, joinTuple);
                }
            }
        }
    }
    
    private readonly IRelation _baseRelation;
    private readonly IRelation _joinRelation;

    public CrossJoinRelation(IRelation baseRelation, IRelation joinRelation)
    {
        _baseRelation = baseRelation;
        _joinRelation = joinRelation;
    }
}