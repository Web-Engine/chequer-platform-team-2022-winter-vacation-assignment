using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions.Functions;

public class MinExpressionNode : IEvaluateExpressionNode
{
    private readonly IExpressionNode _expressionNode;

    public MinExpressionNode(IExpressionNode expressionNode)
    {
        _expressionNode = expressionNode;
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        var value = evaluator.Evaluate(_expressionNode);

        if (value is not IEnumerable<IValue> values)
            return value;

        var min = values.Min();
        if (min is null)
            throw new InvalidOperationException();

        return min;
    }
}