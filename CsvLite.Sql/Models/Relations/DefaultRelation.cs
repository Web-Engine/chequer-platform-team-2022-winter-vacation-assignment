using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Models.Relations;

public class DefaultRelation : IRelation
{
    public IAttributeList Attributes { get; }
    public IReadOnlyList<IRecord> Records { get; }

    public DefaultRelation(IAttributeList attributes, IReadOnlyList<IRecord> records)
    {
        Attributes = attributes;
        Records = records;
    }
}