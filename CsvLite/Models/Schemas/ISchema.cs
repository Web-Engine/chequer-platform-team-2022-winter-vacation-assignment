using CsvLite.Models.Relations;

namespace CsvLite.Models.Schemas;


public interface ISchema : IReadOnlySchema
{
    IReadOnlyDictionary<string, IReadOnlyRelation> IReadOnlySchema.Relations {
        get
        {
            return Relations.ToDictionary(
                pair => pair.Key,
                pair => pair.Value as IReadOnlyRelation
            );
        }
    }
     
    new IDictionary<string, IRelation> Relations { get; }
}
