using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Models.Relations;

public class EmptyRelation : IRelation
{
    public IAttributeList Attributes { get; } = new EmptyAttributeList();

    public IReadOnlyList<IRecord> Records { get; } = new Empty1RecordList();

    private class EmptyAttributeList : List<IAttribute>, IAttributeList
    {
        public IEnumerable<(IAttribute Attribute, int Index)> FindAttributes(IAttributeReference reference)
        {
            throw new InvalidOperationException($"Cannot find attribute with identifier {reference}");
        }
    }

    private class Empty1RecordList : List<IRecord>
    {
        public Empty1RecordList()
        {
            Add(new DefaultRecord());
        }
    }
}