using System.Collections;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvLite.Models.Attributes;
using CsvLite.Models.Domains;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.IO.Csv;

public class CsvRelation : IPhysicalRelation
{
    public IReadOnlyList<IAttribute> Attributes
    {
        get
        {
            _attributes ??= ReadAttributes();

            return _attributes;
        }
    }

    private List<IAttribute>? _attributes;

    public IEnumerable<IRecord> Records
    {
        get
        {
            lock (_cache)
            {
                var domains = Attributes.Select(attribute => attribute.Domain).ToList();

                foreach (var cache in _cache)
                {
                    yield return cache;
                }

                using var enumerator = CsvRecordsList.GetEnumerator();
                for (var i = 0; i < _cache.Count; i++)
                {
                    if (!enumerator.MoveNext()) break;
                }

                for (var i = _cache.Count; i < _cacheLimit; i++)
                {
                    if (!enumerator.MoveNext())
                        break;

                    var record = ParseRecord(enumerator.Current, domains);
                    _cache.Add(record);
                    
                    yield return record;
                }

                while (enumerator.MoveNext())
                    yield return ParseRecord(enumerator.Current, domains);
            }
        }
    }

    private CsvRecordList CsvRecordsList => new(_filePath);

    private readonly string _filePath;

    private readonly List<IRecord> _cache = new(100);
    private readonly int _cacheLimit = 100;

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

        var detectors = Enumerable.Range(0, headerRecord.Length).Select(_ => new DomainDetector()).ToArray();

        foreach (var record in CsvRecordsList.Take(10))
        {
            for (var i = 0; i < headerRecord.Length; i++)
            {
                var value = record[i];

                detectors[i].Detect(value);
            }
        }

        var attributes = headerRecord
            .Zip(detectors)
            .Select((tuple) =>
            {
                var headerName = tuple.First;
                var detector = tuple.Second;

                var identifier = new Identifier(headerName);

                return new DefaultAttribute(identifier, detector.Domain);
            })
            .ToList<IAttribute>();

        return attributes;
    }

    public void AddRecords(IEnumerable<IRecord> records)
    {
        using var streamWriter = new StreamWriter(_filePath, append: true);
        using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        }, leaveOpen: true);

        foreach (var record in records)
        {
            csvWriter.NextRecord();

            foreach (var value in record)
            {
                csvWriter.WriteField(value.AsString().Value);
            }
        }
    }

    private static IRecord ParseRecord(IReadOnlyList<string?> data, IEnumerable<IPrimitiveDomain> domains)
    {
        var values = data
            .Zip(domains)
            .Select(tuple =>
            {
                var value = tuple.First;
                var domain = tuple.Second;

                return domain.Parse(value);
            });

        return new DefaultRecord(values);
    }

    private class CsvRecordList : IEnumerable<IReadOnlyList<string?>>
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

        public IEnumerator<IReadOnlyList<string?>> GetEnumerator()
        {
            return new CsvRecordEnumerator(_filePath);
        }
    }

    private class CsvRecordEnumerator : IEnumerator<IReadOnlyList<string?>>
    {
        private readonly StreamReader _streamReader;
        private readonly CsvReader _csvReader;

        public IReadOnlyList<string?> Current => _current ?? throw new InvalidOperationException();
        object IEnumerator.Current => Current;

        private IReadOnlyList<string?>? _current;
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

            _current = Enumerable.Range(0, _attributeCount)
                .Select(index => _csvReader.GetField(index))
                .ToList();

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