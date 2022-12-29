using CsvLite.Models.Attributes;
using CsvLite.Models.Records;

namespace CsvLite.Models.Relations;

public class DefaultRelation : IRelation
{
    public IAttributeList Attributes { get; }
    public IEnumerable<IRecord> Records { get; }

    public DefaultRelation(IAttributeList attributes, IEnumerable<IRecord> records)
    {
        Attributes = attributes;
        Records = records;
    }
}