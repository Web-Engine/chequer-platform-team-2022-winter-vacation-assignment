using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Attributes;

public class ExpressionAttributeDefinitionNode : IAttributeDefinitionNode
{
    private readonly IExpressionNode _expressionNode;
    private readonly Identifier _name;

    public ExpressionAttributeDefinitionNode(Identifier name, IExpressionNode expressionNode)
    {
        _name = name;
        _expressionNode = expressionNode;
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IRelationEvaluateContext context)
    {
        yield return new DefaultAttribute(
            Identifier.Empty,
            _name
        );
    }

    public IEnumerable<IValue> EvaluateValues(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        yield return evaluator.Evaluate(_expressionNode);
    }
}