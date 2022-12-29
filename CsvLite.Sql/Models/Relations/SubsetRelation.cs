using System.Collections;
using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;
using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Utilities;

namespace CsvLite.Sql.Models.Relations;

public class SubsetRelation : IWritableRelation
{
    public IAttributeList Attributes { get; }

    public IEnumerable<IRecord> Records { get; }

    private readonly IWritableRelation _relation;

    private readonly Dictionary<int, int> _mapToOuter;

    public SubsetRelation(IWritableRelation relation, IEnumerable<Identifier> outerAttributes)
    {
        _relation = relation;

        var innerAttributes = relation.Attributes.ToList();

        var indexMap = outerAttributes
            .WithIndex()
            .Select(item =>
            {
                var (outerAttributeIdentifier, outerIndex) = item;

                var innerIndex = innerAttributes.FindIndex(
                    innerAttribute => innerAttribute.Name.Equals(outerAttributeIdentifier)
                );

                if (innerIndex == -1)
                    throw new InvalidOperationException($"Attribute {item.Value} does not exists");

                return (
                    OuterIndex: outerIndex,
                    InnerIndex: innerIndex
                );
            })
            .ToList();

        _mapToOuter = indexMap.ToDictionary(
            x => x.InnerIndex,
            x => x.OuterIndex
        );

        var mapToInner = indexMap.ToDictionary(
            x => x.OuterIndex,
            x => x.InnerIndex
        );

        Attributes = new SubsetAttributeList(relation.Attributes, mapToInner, _mapToOuter);
        Records = relation.Records.Select(innerRecord =>
        {
            var outerRecord = new DefaultRecord();

            for (var i = 0; i < mapToInner.Count; i++)
            {
                outerRecord.Add(innerRecord[mapToInner[i]]);
            }

            return outerRecord;
        });
    }

    public void AddRecords(IEnumerable<IRecord> outerRecords)
    {
        var innerRecords = outerRecords.Select(outerRecord =>
        {
            var innerIndexes = Enumerable.Range(0, _relation.Attributes.Count);

            var innerValues = innerIndexes.Select(innerIndex =>
            {
                if (_mapToOuter.TryGetValue(innerIndex, out var outerIndex))
                    return outerRecord[outerIndex];

                return NullValue.Null;
            });
            
            var innerRecord = new DefaultRecord(innerValues);
            return innerRecord;
        });

        _relation.AddRecords(innerRecords);
    }

    private class SubsetAttributeList : IAttributeList
    {
        public int Count => _mapToInner.Count;

        public IAttribute this[int outerIndex]
        {
            get
            {
                var innerIndex = _mapToInner[outerIndex];

                return _original[innerIndex];
            }
        }

        private readonly IAttributeList _original;

        private readonly IReadOnlyDictionary<int, int> _mapToInner;
        private readonly IReadOnlyDictionary<int, int> _mapToOuter;

        public SubsetAttributeList(
            IAttributeList original,
            IReadOnlyDictionary<int, int> mapToInner,
            IReadOnlyDictionary<int, int> mapToOuter
        )
        {
            _original = original;
            _mapToInner = mapToInner;
            _mapToOuter = mapToOuter;
        }

        public IEnumerable<(IAttribute Attribute, int Index)> FindAttributes(IAttributeReference reference)
        {
            return _original.FindAttributes(reference)
                .Select(x => (x.Attribute, _mapToOuter[x.Index]));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IAttribute> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        private IEnumerable<IAttribute> Enumerate()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }
    }
}