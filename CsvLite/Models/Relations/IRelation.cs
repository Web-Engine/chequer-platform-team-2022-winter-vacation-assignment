using CsvLite.Models.Attributes;
using CsvLite.Models.Records;

namespace CsvLite.Models.Relations;

public interface IRelation
{
    IAttributeList Attributes { get; }

    IEnumerable<IRecord> Records { get; }
}
