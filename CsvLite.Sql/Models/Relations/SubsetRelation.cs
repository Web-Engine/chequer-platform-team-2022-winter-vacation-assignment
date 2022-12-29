using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;
using CsvLite.Models.Values.Primitives;

namespace CsvLite.Sql.Models.Relations;

public class SubsetRelation : IWritableRelation
{
    public IReadOnlyList<IAttribute> Attributes { get; }

    public IEnumerable<IRecord> Records { get; }

    private readonly IWritableRelation _relation;

    private readonly Dictionary<int, int> _mapToOuter;

    public SubsetRelation(IWritableRelation relation, IReadOnlyList<(int InnerIndex, int OuterIndex)> attributeMap)
    {
        _relation = relation;

        _mapToOuter = attributeMap
            .ToDictionary(x => x.OuterIndex, x => x.InnerIndex);

        Attributes = attributeMap
            .Select(map => relation.Attributes[map.OuterIndex])
            .ToList();

        Records = relation.Records
            .Select(record =>
            {
                return new DefaultRecord(
                    attributeMap
                        .Select(map => record[map.OuterIndex])
                );
            });
    }

    public void AddRecords(IEnumerable<IRecord> outerRecords)
    {
        var innerRecords = outerRecords.Select(outerRecord =>
        {
            var innerValues = Enumerable.Range(0, _relation.Attributes.Count)
                .Select(innerIndex =>
                {
                    if (_mapToOuter.TryGetValue(innerIndex, out var outerIndex))
                        return outerRecord[outerIndex];

                    return NullValue.Null;
                });

            return new DefaultRecord(innerValues);
        });

        _relation.AddRecords(innerRecords);
    }
}