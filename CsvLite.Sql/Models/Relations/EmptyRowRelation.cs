using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Models.Relations;

public class EmptyRowRelation : IRelation
{
    public IAttributeList Attributes { get; } = new DefaultAttributeList();

    public IEnumerable<IRecord> Records { get; } = new List<IRecord>
    {
        new DefaultRecord()
    };
}