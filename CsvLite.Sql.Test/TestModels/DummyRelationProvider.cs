using System;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Test.TestModels;

public class DummyRelationProvider : IRelationProvider
{
    public IPhysicalRelation GetRelation(Identifier identifier)
    {
        switch (identifier.Value.ToUpper())
        {
            case "tableA":
                return new DummyRelation(
                    new[] {"colA", "colB", "colC", "colD"},
                    10,
                    (i) => new DummyRecord(
                        $"value{i}",
                        $"value{i}",
                        $"value{i}",
                        $"value{i}"
                    )
                );
        }

        throw new InvalidOperationException($"Cannot find a relation {identifier}");
    }
}