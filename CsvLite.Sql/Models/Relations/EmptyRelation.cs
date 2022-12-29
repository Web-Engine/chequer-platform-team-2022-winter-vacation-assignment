using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Models.Attributes;

namespace CsvLite.Sql.Models.Relations;

public class EmptyRelation : IRelation
{
    public IAttributeList Attributes { get; } = new DefaultAttributeList();

    public IEnumerable<IRecord> Records { get; } = new List<IRecord>();
}