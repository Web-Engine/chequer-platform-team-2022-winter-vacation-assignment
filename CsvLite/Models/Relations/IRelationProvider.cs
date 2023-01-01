using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Relations;

public interface IRelationProvider
{
    IPhysicalRelation GetRelation(Identifier identifier);
}
