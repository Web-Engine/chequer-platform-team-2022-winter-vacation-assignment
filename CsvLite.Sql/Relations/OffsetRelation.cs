using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Relations;

internal class OffsetRelation : IRelation
{
    public IEnumerable<IAttribute> Attributes => _relation.Attributes;

    public IEnumerable<ITuple> Tuples => _relation.Tuples.Skip(_offset);
    
    private readonly IRelation _relation;
    private readonly int _offset;

    public OffsetRelation(IRelation relation, int offset)
    {
        _relation = relation;
        _offset = offset;
    }
}
