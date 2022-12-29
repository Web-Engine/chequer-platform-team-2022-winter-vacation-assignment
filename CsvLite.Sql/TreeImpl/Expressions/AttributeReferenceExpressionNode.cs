using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.TreeImpl.Attributes;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public sealed class AttributeReferenceExpressionNode : IExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return ReferenceNode; }
    }

    public NodeValue<IAttributeReferenceNode> ReferenceNode { get; }

    public AttributeReferenceExpressionNode(IAttributeReferenceNode attributeReferenceNode)
    {
        ReferenceNode = attributeReferenceNode.ToNodeValue();
    }

    public IValue Evaluate(IRecordContext context)
    {
        var values = ReferenceNode.Value.GetValues(context, out _).Take(2).ToList();

        if (ReferenceNode.Value.Reference is AllAttributeReference)
            return new TupleValue(values);

        return values.Count switch
        {
            0 => throw new InvalidOperationException($"Attribute not found {ReferenceNode.Value.Reference}"),
            1 => values[0],
            _ => throw new InvalidOperationException($"Attribute ambiguous {ReferenceNode.Value.Reference}"),
        };
    }
}