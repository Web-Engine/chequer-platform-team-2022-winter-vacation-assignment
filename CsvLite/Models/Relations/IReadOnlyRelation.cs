using CsvLite.Models.Attributes;

namespace CsvLite.Models.Relations;

public interface IReadOnlyRelation
{
    public IEnumerable<IReadOnlyAttribute> Attributes { get; }
}