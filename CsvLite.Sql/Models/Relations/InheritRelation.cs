using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Models.Relations;

public class InheritRelation : IRelation
{
    public IAttributeList Attributes => _attributes ?? _relation.Attributes;

    public IEnumerable<IRecord> Records => _records ?? _relation.Records;

    private readonly IRelation _relation;
    private readonly IAttributeList? _attributes;
    private readonly IEnumerable<IRecord>? _records;

    public InheritRelation(IRelation relation, IAttributeList? attributes = null, IEnumerable<IRecord>? records = null)
    {
        _relation = relation;
        _attributes = attributes;
        _records = records;
    }
}