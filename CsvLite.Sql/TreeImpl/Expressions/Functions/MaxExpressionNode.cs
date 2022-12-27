using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions.Functions;

public class MaxExpressionNode : IEvaluateExpressionNode
{
    private readonly IExpressionNode _expressionNode;

    public MaxExpressionNode(IExpressionNode expressionNode)
    {
        _expressionNode = expressionNode;
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        var value = evaluator.Evaluate(_expressionNode);

        if (value is not IEnumerable<IValue> values)
            return value;

        var max = values.Max();
        if (max is null)
            throw new InvalidOperationException();

        return max;
    }
}