using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class BinaryCalculateExpressionNode : IEvaluateExpressionNode
{
    private readonly IExpressionNode _expression1;
    private readonly IExpressionNode _expression2;

    private readonly BinaryCalculateOperator _operator;

    public enum BinaryCalculateOperator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Modulus
    }

    public BinaryCalculateExpressionNode(
        IExpressionNode expression1,
        BinaryCalculateOperator @operator,
        IExpressionNode expression2
    )
    {
        _expression1 = expression1;
        _operator = @operator;
        _expression2 = expression2;
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        
        var value1 = evaluator.Evaluate(_expression1);
        var value2 = evaluator.Evaluate(_expression2);

        return (value1, value2) switch
        {
            (NullValue, _) => throw new InvalidOperationException("Cannot calculate with DbNull"),
            (_, NullValue) => throw new InvalidOperationException("Cannot calculate with DbNull"),
            
            (IntegerValue int1, IntegerValue int2) => EvaluateInteger(int1, int2),

            (StringValue str1, StringValue str2) => EvaluateString(str1, str2),
            (StringValue str1, _) => EvaluateString(str1, value2.AsString()),
            (_, StringValue str2) => EvaluateString(value1.AsString(), str2),
            
            (_, _) => throw new InvalidOperationException("Cannot calculate")
        };
    }

    private IntegerValue EvaluateInteger(IntegerValue value1, IntegerValue value2)
    {
        return new IntegerValue(_operator switch
        {
            BinaryCalculateOperator.Addition => value1.Value + value2.Value,
            BinaryCalculateOperator.Subtraction => value1.Value - value2.Value,
            BinaryCalculateOperator.Multiplication => value1.Value * value2.Value,
            BinaryCalculateOperator.Division => value1.Value / value2.Value,
            BinaryCalculateOperator.Modulus => value1.Value % value2.Value,

            _ => throw new InvalidOperationException($"Invalid operator {_operator.ToString()}")
        });
    }

    private StringValue EvaluateString(StringValue value1, StringValue value2)
    {
        return new StringValue(_operator switch
        {
            BinaryCalculateOperator.Addition => value1.Value + value2.Value,

            _ => throw new InvalidOperationException($"Invalid string operator {_operator.ToString()}")
        });
    }
}