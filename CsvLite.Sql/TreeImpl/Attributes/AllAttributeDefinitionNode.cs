using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Attributes;

public class AllAttributeDefinitionNode : IAttributeDefinitionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return AttributeReferenceNode; }
    }

    public NodeValue<IAttributeReferenceNode> AttributeReferenceNode { get; }

    public AllAttributeDefinitionNode(IAttributeReferenceNode attributeReferenceNode)
    {
        AttributeReferenceNode = attributeReferenceNode.ToNodeValue();
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IRelationContext context)
    {
        return AttributeReferenceNode.Value.GetAttributes(context, out _);
    }

    public IEnumerable<IValue> EvaluateValues(IRecordContext context)
    {
        return AttributeReferenceNode.Value.GetValues(context, out _);
    }
}