using System.Globalization;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.IO.Csv;

public class CsvRelationProvider : IRelationProvider
{
    IPhysicalRelation IRelationProvider.GetRelation(Identifier identifier) => GetRelation(identifier);

    public CsvRelation GetRelation(Identifier identifier)
    {
        return new CsvRelation(identifier);
    }
}