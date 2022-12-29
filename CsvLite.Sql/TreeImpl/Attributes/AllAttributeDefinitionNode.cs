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
        get { yield return ExpressionNode; }
    }

    public NodeValue<IAllAttributeReferenceExpressionNode> ExpressionNode { get; }

    public AllAttributeDefinitionNode(IAllAttributeReferenceExpressionNode expressionNode)
    {
        ExpressionNode = expressionNode.ToNodeValue();
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IRelationContext context)
    {
        var indexedAttributes = context.Relation.Attributes
            .FindAttributes(ExpressionNode.Value.AttributeReference);

        return indexedAttributes.Select(x => x.Attribute);
    }

    public IEnumerable<IValue> EvaluateValues(IRecordContext context)
    {
        var value = ExpressionNode.Value.Evaluate(context);

        if (value is not TupleValue tuple)
            throw new InvalidOperationException("DB ERROR Occurred");

        return tuple;
    }
}