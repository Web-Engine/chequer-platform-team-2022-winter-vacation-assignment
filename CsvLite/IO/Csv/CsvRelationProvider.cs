using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.IO.Csv;

public class CsvRelationProvider : IPhysicalRelationProvider
{
    public IPhysicalRelation GetRelation(Identifier identifier)
    {
        using var reader = new CsvReader(identifier.Value);

        return reader.Read();
    }
}