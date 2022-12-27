using CsvLite.IO.Relations;
using CsvLite.Models.Relations;
using CsvLite.Models.Values;
using CsvLite.Utilities;

namespace CsvLite.IO.Csv;

public sealed class CsvWriter : IRelationWriter, IDisposable
{
    private readonly TextWriter _writer;
    private const string Separator = ",";

    public CsvWriter(string filePath)
    {
        _writer = File.CreateText(filePath);
    }

    public void Write(IRelation relation)
    {
        WriteRow(relation.Attributes.Select(x => x.Name.Value));

        foreach (var record in relation.Records)
        {
            WriteRow(record.Select(ConvertRecord));
        }
    }

    private void WriteRow(IEnumerable<string> values)
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

    private static string ConvertRecord(IValue value)
    {
        return value switch
        {
            StringValue stringValue => stringValue.Value,
            IntegerValue integerValue => integerValue.Value.ToString(),
            BooleanValue booleanValue => booleanValue.Value ? "TRUE" : "FALSE",

            _ => throw new NotSupportedException($"Cannot save {value.GetType()} to csv")
        };
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}