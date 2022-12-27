using CsvLite.Utilities;

namespace CsvLite.Models.Attributes;

public sealed class DefaultAttributeList : List<IAttribute>, IAttributeList
{
    public DefaultAttributeList()
    {
    }

    public DefaultAttributeList(IEnumerable<IAttribute> newAttributes) : base(newAttributes)
    {
    }

    IEnumerator<IAttribute> IEnumerable<IAttribute>.GetEnumerator() => GetEnumerator();
    IAttribute IReadOnlyList<IAttribute>.this[int index] => this[index];

    public IEnumerable<(IAttribute Attribute, int Index)> FindAttributes(IAttributeReference reference)
    {
        return this
            .WithIndex()
            .Where(indexedValue => reference.IsReferencing(indexedValue.Value));
    }
}