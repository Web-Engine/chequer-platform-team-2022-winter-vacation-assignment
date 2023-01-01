using System;
using System.Collections.Generic;
using System.Linq;
using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Test.TestModels;

public class DummyRelation : IPhysicalRelation
{
    IReadOnlyList<IAttribute> IRelation.Attributes => Attributes;

    IEnumerable<IRecord> IRelation.Records => Records;

    public List<IAttribute> Attributes { get; }
    public List<IRecord> Records { get; }

    public DummyRelation(IEnumerable<string> attributes, int count, Func<int, IRecord> recordFactory)
    {
        Attributes = attributes
            .Select(attribute => new Identifier(attribute))
            .Select(identifier => new DefaultAttribute(identifier))
            .ToList<IAttribute>();

        Records = Enumerable.Range(0, count).Select(recordFactory).ToList();
    }

    public DummyRelation(IEnumerable<IAttribute> attributes, IEnumerable<IRecord> records)
    {
        Attributes = attributes.ToList();
        Records = records.ToList();
    }

    public void AddRecords(IEnumerable<IRecord> records)
    {
        Records.AddRange(records);
    }
}