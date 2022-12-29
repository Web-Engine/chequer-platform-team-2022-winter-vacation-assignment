using CsvLite.IO.Relations;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;
using CsvLite.Utilities;

namespace CsvLite.IO.Csv;

public sealed class MyCsvWriter : IRelationWriter, IDisposable
{
    private readonly TextWriter _writer;
    private const string Separator = ",";

    public MyCsvWriter(string filePath)
    {
        _writer = File.AppendText(filePath);
    }

    public void Write(IRelation relation)
    {
        WriteRow(relation.Attributes.Select(x => x.Name.Value));

        foreach (var record in relation.Records)
        {
            WriteRecord(record);
        }
    }
    public void WriteRecord(IRecord record)
    {
        WriteRow(record.Select(value => value.AsString().Value));
    }

    public void WriteRow(IEnumerable<string> values)
    {
        using var enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            _writer.WriteLine();
            return;
        }

        WriteValue(enumerator.Current);

        while (enumerator.MoveNext())
        {
            _writer.Write(Separator);

            WriteValue(enumerator.Current);
        }

        _writer.WriteLine();
    }

    private void WriteValue(string value)
    {
        if (!RequireEscape(value))
        {
            _writer.Write(value);
            return;
        }
        
        var escapedValue = StringUtility.Escape(value, '"', "\"\"");
        _writer.Write(escapedValue);
    }

    private bool RequireEscape(string value)
    {
        return value.Any(x => x is '"' or ',');
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}