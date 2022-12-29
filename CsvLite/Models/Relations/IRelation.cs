using CsvLite.Models.Attributes;
using CsvLite.Models.Records;

namespace CsvLite.Models.Relations;

public interface IRelation
{
    IReadOnlyList<IAttribute> Attributes { get; }

    IEnumerable<IRecord> Records { get; }
}
