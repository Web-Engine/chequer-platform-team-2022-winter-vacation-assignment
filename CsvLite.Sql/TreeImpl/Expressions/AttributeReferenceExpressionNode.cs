using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public sealed class AttributeReferenceExpressionNode : IExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return ReferenceNode; }
    }

    public NodeValue<IExplicitAttributeReferenceNode> ReferenceNode { get; }

    public AttributeReferenceExpressionNode(IExplicitAttributeReferenceNode referenceNode)
    {
        ReferenceNode = referenceNode.ToNodeValue();
    }

    public IDomain EvaluateDomain(IRelationContext context)
    {
        var attribute = ReferenceNode.Value.GetAttribute(context, out _);

        return attribute.Domain;
    }

    public IValue EvaluateValue(IRecordContext context)
    {
        var value = ReferenceNode.Value.GetValue(context, out _);

        return value;
    }
}