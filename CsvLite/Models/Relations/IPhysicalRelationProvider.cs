using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Relations;

public interface IPhysicalRelationProvider
{
    IPhysicalRelation GetRelation(Identifier identifier);
}
