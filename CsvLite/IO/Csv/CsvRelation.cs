using System.Collections;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Models.Values;
using CsvLite.Utilities;

namespace CsvLite.IO.Csv;

public class CsvRelation : IPhysicalRelation
{
    private const int CacheLimit = 100;
    
    public IReadOnlyList<IAttribute> Attributes
    {
        get
        {
            _attributes ??= ReadAttributes();

            return _attributes;
        }
    }

    private List<IAttribute>? _attributes;

    public IEnumerable<IRecord> Records => CsvRecordsList.Cache(_cache, CacheLimit);

    private CsvRecordList CsvRecordsList => new(_filePath);

    private readonly string _filePath;

    private readonly List<IRecord> _cache = new(CacheLimit);

    public CsvRelation(Identifier identifier)
    {
        _filePath = identifier.Value;
    }

    private List<IAttribute> ReadAttributes()
    {
        using var streamReader = new StreamReader(_filePath);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        csvReader.Read();
        csvReader.ReadHeader();

        if (csvReader.HeaderRecord is not { } headerRecord)
            throw new InvalidOperationException("Missing CSV Header Records");

        var attributes = new List<IAttribute>(
            headerRecord.Select(name =>
            {
                var attributeIdentifier = new Identifier(name);

                return new DefaultAttribute(attributeIdentifier);
            })
        );

        return attributes;
    }

    public void AddRecords(IEnumerable<IRecord> records)
    {
        using var streamWriter = new StreamWriter(_filePath, append: true);
        using var csvWriter = new CsvWriter(
            streamWriter,
            new CsvConfiguration(CultureInfo.InvariantCulture) {HasHeaderRecord = false},
            leaveOpen: true
        );

        foreach (var record in records)
        {
            csvWriter.NextRecord();

            foreach (var value in record)
            {
                csvWriter.WriteField(value.AsString().Value);
            }
        }
    }

    private class CsvRecordList : IEnumerable<IRecord>
    {
        private readonly string _filePath;

        public CsvRecordList(string filePath)
        {
            _filePath = filePath;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IRecord> GetEnumerator()
        {
            return new CsvRecordEnumerator(_filePath);
        }
    }

    private class CsvRecordEnumerator : IEnumerator<IRecord>
    {
        private readonly StreamReader _streamReader;
        private readonly CsvReader _csvReader;

        public IRecord Current => _current ?? throw new InvalidOperationException();
        object IEnumerator.Current => Current;

        private IRecord? _current;
        private readonly int _attributeCount;

        public CsvRecordEnumerator(string filePath)
        {
            _streamReader = new StreamReader(filePath);
            _csvReader = new CsvReader(_streamReader, CultureInfo.InvariantCulture, leaveOpen: true);

            // Skip attributes
            _csvReader.Read();
            _csvReader.ReadHeader();

            _attributeCount = _csvReader.HeaderRecord?.Length ?? 0;
        }

        public bool MoveNext()
        {
            var read = _csvReader.Read();
            if (!read) return false;

            _current = new DefaultRecord(
                Enumerable.Range(0, _attributeCount)
                    .Select(index => _csvReader.GetField(index))
                    .Select(PrimitiveValue.Parse)
            );

            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            _csvReader.Dispose();
            _streamReader.Dispose();
        }
    }
}