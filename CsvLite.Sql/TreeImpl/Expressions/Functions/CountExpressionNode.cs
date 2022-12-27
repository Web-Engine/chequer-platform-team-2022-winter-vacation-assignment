using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions.Functions;

public class CountExpressionNode : IEvaluateExpressionNode
{
    private readonly IExpressionNode _expressionNode;
    private readonly bool _distinct;

    public CountExpressionNode(IExpressionNode expressionNode, bool distinct)
    {
        _expressionNode = expressionNode;
        _distinct = distinct;
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        var value = evaluator.Evaluate(_expressionNode);

        if (value is not IEnumerable<IValue> values)
            return new IntegerValue(1);

        if (!_distinct)
            values = values.Distinct();

        var count = values.Count();

        return new IntegerValue(count);
    }
}