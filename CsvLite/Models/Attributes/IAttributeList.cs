namespace CsvLite.Models.Attributes;

public interface IAttributeList : IReadOnlyList<IAttribute>
{
    IEnumerable<(IAttribute Attribute, int Index)> FindAttributes(IAttributeReference reference);
}