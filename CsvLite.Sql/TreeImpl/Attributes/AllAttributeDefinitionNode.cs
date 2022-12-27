using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Attributes;

public class AllAttributeDefinitionNode : IAttributeDefinitionNode
{
    private readonly IAllAttributeReferenceExpressionNode _expressionNode;

    public AllAttributeDefinitionNode(IAllAttributeReferenceExpressionNode expressionNode)
    {
        _expressionNode = expressionNode;
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IRelationEvaluateContext context)
    {
        var indexedAttributes = context.Relation.Attributes
            .FindAttributes(_expressionNode.AttributeReference);

        return indexedAttributes.Select(x => x.Attribute);
    }

    public IEnumerable<IValue> EvaluateValues(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        var value = evaluator.Evaluate(_expressionNode);

        if (value is not TupleValue tuple)
            throw new InvalidOperationException("DB ERROR Occurred");
        
        return tuple.Values;
    }
}