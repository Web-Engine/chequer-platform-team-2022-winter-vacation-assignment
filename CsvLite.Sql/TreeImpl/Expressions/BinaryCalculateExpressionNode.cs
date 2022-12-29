using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class BinaryCalculateExpressionNode : IPrimitiveExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get
        {
            yield return ExpressionNode1;
            yield return ExpressionNode2;
        }
    }

    public NodeValue<IExpressionNode> ExpressionNode1 { get; }
    public NodeValue<IExpressionNode> ExpressionNode2 { get; }

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
        IExpressionNode expressionNode1,
        BinaryCalculateOperator @operator,
        IExpressionNode expressionNode2
    )
    {
        ExpressionNode1 = expressionNode1.ToNodeValue();
        _operator = @operator;
        ExpressionNode2 = expressionNode2.ToNodeValue();
    }

    public PrimitiveValue Evaluate(IRecordContext context)
    {
        var value1 = ExpressionNode1.Evaluate(context).AsPrimitive();
        var value2 = ExpressionNode2.Evaluate(context).AsPrimitive();

        return Evaluate(value1, value2);
    }

    public PrimitiveValue Evaluate(PrimitiveValue value1, PrimitiveValue value2)
    {
        return (value1, value2) switch
        {
            (NullValue, _) => throw new InvalidOperationException("Cannot calculate with DbNull"),
            (_, NullValue) => throw new InvalidOperationException("Cannot calculate with DbNull"),

            (IntegerValue integerValue1, IntegerValue integerValue2) => EvaluateInteger(integerValue1, integerValue2),

            (StringValue stringValue1, StringValue stringValue2) => EvaluateString(stringValue1, stringValue2),
            (StringValue stringValue1, _) => EvaluateString(stringValue1, value2.AsString()),
            (_, StringValue stringValue2) => EvaluateString(value1.AsString(), stringValue2),

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