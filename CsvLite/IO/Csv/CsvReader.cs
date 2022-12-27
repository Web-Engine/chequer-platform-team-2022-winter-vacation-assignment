using System.Text;
using CsvLite.IO.Relations;
using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Models.Values;

namespace CsvLite.IO.Csv;

public sealed class CsvReader : IRelationReader, IDisposable
{
    private readonly string _filePath;

    private readonly StreamReader _reader;

    public CsvReader(string filePath)
    {
        _filePath = filePath;

        _reader = File.OpenText(_filePath);
    }

    public IPhysicalRelation Read()
    {
        var relationIdentifier = new Identifier(_filePath);
        var attributeLine = _reader.ReadLine();

        if (attributeLine is null)
        {
            return new CsvRelation(relationIdentifier,
                new DefaultAttributeList(),
                new List<IRecord>()
            );
        }

        var attributes = ReadRow(attributeLine).Select(name =>
        {
            var attributeIdentifier = new Identifier(name);
            return new DefaultAttribute(relationIdentifier, attributeIdentifier);
        });

        var attributeList = new DefaultAttributeList(attributes);

        var recordList = new List<DefaultRecord>();
        while (true)
        {
            var line = _reader.ReadLine();
            if (line is null) break;

            var values = ReadRow(line).Select(value => new StringValue(value));
            
            var record = new DefaultRecord(values);
            if (record.Count == 0)
                continue;

            recordList.Add(record);
        }

        return new CsvRelation(relationIdentifier, attributeList, recordList);
    }

    private IEnumerable<string> ReadRow(string line)
    {
        var span = line.AsSpan();

        var values = new List<string>();

        while (!span.IsEmpty)
        {
            values.Add(ReadValue(ref span));
        }

        return values;
    }

    private string ReadValue(ref ReadOnlySpan<char> span)
    {
        SkipWhitespaces(ref span);

        if (span.IsEmpty)
            return string.Empty;

        if (span[0] == '"')
            return ReadEscapedValue(ref span);

        return ReadUnescapedValue(ref span);
    }

    private void SkipWhitespaces(ref ReadOnlySpan<char> span)
    {
        while (!span.IsEmpty)
        {
            if (span[0] is not ' ') break;
            
            span = span[1..];
        }
    }

    private string ReadEscapedValue(ref ReadOnlySpan<char> span)
    {
        span = span[1..];

        var builder = new StringBuilder();
        while (!span.IsEmpty)
        {
            if (span.StartsWith("\"\""))
            {
                builder.Append('"');
                span = span[2..];
                continue;
            }

            if (span[0] == '"')
            {
                span = span[1..];
                break;
            }

            builder.Append(span[0]);
            span = span[1..];
        }

        SkipWhitespaces(ref span);

        if (span.IsEmpty) return builder.ToString();
        
        if (span[0] != ',')
            throw new InvalidOperationException("Invalid Csv file format");
          
        span = span[1..];
        return builder.ToString();
    }

    private string ReadUnescapedValue(ref ReadOnlySpan<char> span)
    {
        var commaIndex = span.IndexOf(",");
        if (commaIndex == -1)
        {
            var str = span.ToString();
            span = ReadOnlySpan<char>.Empty;

            return str;
        }

        var returnValue = span[..commaIndex].ToString().TrimEnd();
        span = span[(commaIndex + 1)..];

        return returnValue;
    }

    public void Dispose()
    {
        _reader.Dispose();
    }
}