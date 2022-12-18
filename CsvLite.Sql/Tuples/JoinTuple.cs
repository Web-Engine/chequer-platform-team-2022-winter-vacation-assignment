using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Tuples;

public class JoinTuple : ITuple
{
    public IEnumerable<object> Values
    {
        get
        {
            foreach (var value in _baseTuple.Values)
            {
                yield return value;
            }

            foreach (var value in _addTuple.Values)
            {
                yield return value;
            }
        }
    }
    
    private readonly ITuple _baseTuple;
    private readonly ITuple _addTuple;
    
    public JoinTuple(ITuple baseTuple, ITuple addTuple)
    {
        _baseTuple = baseTuple;
        _addTuple = addTuple;
    }
}