using CsvLite.Models.Identifiers;

namespace CsvLite.Models.Relations;

public interface IRelationProvider
{
    IRelation GetRelation(Identifier identifier);
}