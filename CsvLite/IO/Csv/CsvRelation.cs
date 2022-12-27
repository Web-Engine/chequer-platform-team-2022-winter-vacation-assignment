using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.IO.Csv;

public class CsvRelation : IPhysicalRelation
{
    IAttributeList IRelation.Attributes => _attributes;
    IReadOnlyList<IRecord> IRelation.Records => _records;

    private readonly Identifier _identifier;
    private readonly DefaultAttributeList _attributes = new();
    private readonly List<IRecord> _records = new();

    public CsvRelation(Identifier identifier, IEnumerable<IAttribute> attributes, IEnumerable<IRecord> records)
    {
        _identifier = identifier;
        _attributes.AddRange(attributes);
        _records.AddRange(records);
    }

    public void AddRecord(IRecord record)
    {
        _records.Add(record);
        Save();
    }

    public void AddRecordRange(IEnumerable<IRecord> records)
    {
        _records.AddRange(records);
        Save();
    }

    private void Save()
    {
        using var writer = new CsvWriter(_identifier.Value);

        writer.Write(this);
    }
}