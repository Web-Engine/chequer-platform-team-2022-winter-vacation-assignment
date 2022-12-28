using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.IO.Csv;

public class CsvRelationProvider : IRelationProvider
{
    IRelation IRelationProvider.GetRelation(Identifier identifier) => GetRelation(identifier);

    public CsvRelation GetRelation(Identifier identifier)
    {
        using var reader = new CsvReader(identifier.Value);

        return reader.Read();
    }
}