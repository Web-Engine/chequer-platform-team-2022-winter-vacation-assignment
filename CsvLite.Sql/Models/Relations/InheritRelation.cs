using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Models.Relations;

public class InheritRelation : IRelation
{
    private readonly IRelation _relation;
    
    public IAttributeList Attributes => _relation.Attributes;

    public IReadOnlyList<IRecord> Records { get; }

    public InheritRelation(IRelation relation, IReadOnlyList<IRecord> records)
    {
        _relation = relation;
        Records = records;
    }
}