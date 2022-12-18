using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Relations;

internal class LimitRelation : IRelation
{
    public IEnumerable<IAttribute> Attributes => _relation.Attributes;

    public IEnumerable<ITuple> Tuples => _relation.Tuples.Take(_limit);

    private readonly IRelation _relation;
    private readonly int _limit;    

    public LimitRelation(IRelation relation, int limit)
    {
        _relation = relation;
        _limit = limit;
    }
}