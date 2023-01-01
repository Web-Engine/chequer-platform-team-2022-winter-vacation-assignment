using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Attributes;

namespace CsvLite.Sql.Tree.Attributes;

public interface IExplicitAttributeReferenceNode : IAttributeReferenceNode
{
    IEnumerable<IAttribute> IAttributeReferenceNode.GetAttributes(IRelationContext context, out IRelationContext found)
    {
        var attribute = GetAttribute(context, out found);

        return Enumerable.Repeat(attribute, 1);
    }

    IEnumerable<int> IAttributeReferenceNode.GetAttributeIndexes(IRelationContext context, out IRelationContext found)
    {
        var index = GetAttributeIndex(context, out found);

        return Enumerable.Repeat(index, 1);
    }

    IEnumerable<IValue> IAttributeReferenceNode.GetValues(IRecordContext context, out IRecordContext found)
    {
        var value = GetValue(context, out found);

        return Enumerable.Repeat(value, 1);
    }

    IAttribute GetAttribute(IRelationContext context, out IRelationContext found);
    int GetAttributeIndex(IRelationContext context, out IRelationContext found);
    IValue GetValue(IRecordContext context, out IRecordContext found);
}