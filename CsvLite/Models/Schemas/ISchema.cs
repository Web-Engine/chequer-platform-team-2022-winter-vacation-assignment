using CsvLite.Models.Relations;

namespace CsvLite.Models.Schemas;


public interface ISchema
{
    IDictionary<string, IRelation> Relations { get; }
}
