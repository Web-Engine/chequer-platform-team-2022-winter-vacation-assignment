using CsvLite.Models.Relations;

namespace CsvLite.Models.Schemas;

public interface IReadOnlySchema
{
    IReadOnlyDictionary<string, IReadOnlyRelation> Relations { get; }
}