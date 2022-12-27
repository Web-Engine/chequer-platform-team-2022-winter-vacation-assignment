using CsvLite.Models.Attributes;
using CsvLite.Utilities;

namespace CsvLite.Sql.Models.Attributes;

public class ConcatAttributeList : List<IAttribute>, IAttributeList
{
    private readonly IAttributeList _attributeList1;
    private readonly IAttributeList _attributeList2;

    public ConcatAttributeList(IAttributeList attributeList1, IAttributeList attributeList2)
    {
        _attributeList1 = attributeList1;
        _attributeList2 = attributeList2;

        AddRange(attributeList1);
        AddRange(attributeList2);
    }

    public IEnumerable<(IAttribute Attribute, int Index)> FindAttributes(IAttributeReference reference)
    {
        var found1 = _attributeList1.FindAttributes(reference).Select(result => result.Attribute);
        var found2 = _attributeList2.FindAttributes(reference).Select(result => result.Attribute);

        return found1.Concat(found2).WithIndex();
    }
}