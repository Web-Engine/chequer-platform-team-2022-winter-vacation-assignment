using CsvLite.Models.Attributes;
using CsvLite.Models.Records;

namespace CsvLite.Models.Relations;

public class DefaultRelation : IRelation
{
    public IReadOnlyList<IAttribute> Attributes { get; }
    public IEnumerable<IRecord> Records { get; }

    public DefaultRelation(IEnumerable<IAttribute> attributes, IEnumerable<IRecord> records)
    {
        Attributes = attributes.ToList();
        Records = records;
    }
}