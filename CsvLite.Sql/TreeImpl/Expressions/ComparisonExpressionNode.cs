using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class ComparisonExpressionNode : IPrimitiveExpressionNode
{
    public enum ComparisonOperator
    {
        Equal,
        NotEqual,

        GreaterThan,
        GreaterThanOrEqual,

        LessThan,
        LessThanOrEqual,
    }

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

    private readonly ComparisonOperator _operator;

    public ComparisonExpressionNode(IExpressionNode expressionNode1, ComparisonOperator @operator,
        IExpressionNode expressionNode2)
    {
        ExpressionNode1 = expressionNode1.ToNodeValue();
        _operator = @operator;
        ExpressionNode2 = expressionNode2.ToNodeValue();
    }

    PrimitiveValue IPrimitiveExpressionNode.Evaluate(IRecordContext context) => Evaluate(context);

    public BooleanValue Evaluate(IRecordContext context)
    {
        var value1 = ExpressionNode1.Evaluate(context).AsPrimitive();
        var value2 = ExpressionNode2.Evaluate(context).AsPrimitive();

        return Evaluate(value1, value2);
    }

    public BooleanValue Evaluate(PrimitiveValue value1, PrimitiveValue value2)
    {
        return (value1, value2) switch
        {
            (NullValue, NullValue) => EvaluateNull(),
            (BooleanValue bool1, BooleanValue bool2) => EvaluateInteger(bool1.AsInteger(), bool2.AsInteger()),
            (IntegerValue int1, IntegerValue int2) => EvaluateInteger(int1, int2),
            (StringValue str1, StringValue str2) => EvaluateString(str1, str2),

            _ => throw new IOException("Cannot compare")
        };
    }

    private BooleanValue EvaluateNull()
    {
        return new BooleanValue(_operator switch
        {
            ComparisonOperator.Equal => true,
            ComparisonOperator.GreaterThanOrEqual => true,
            ComparisonOperator.LessThanOrEqual => true,

            ComparisonOperator.NotEqual => false,
            ComparisonOperator.GreaterThan => false,
            ComparisonOperator.LessThan => false,

            _ => throw new InvalidOperationException("Unknown comparison operator")
        });
    }

    private BooleanValue EvaluateInteger(IntegerValue integer1, IntegerValue integer2)
    {
        var compareTo = integer1.Value.CompareTo(integer2.Value);

        return EvaluateCompareTo(compareTo);
    }

    private BooleanValue EvaluateString(StringValue str1, StringValue str2)
    {
        var result = string.CompareOrdinal(str1.Value, str2.Value);

        return EvaluateCompareTo(result);
    }

    private BooleanValue EvaluateCompareTo(int compareToResult)
    {
        return new BooleanValue(_operator switch
        {
            ComparisonOperator.Equal => compareToResult == 0,
            ComparisonOperator.NotEqual => compareToResult != 0,

            ComparisonOperator.GreaterThan => compareToResult > 0,
            ComparisonOperator.GreaterThanOrEqual => compareToResult >= 0,
            ComparisonOperator.LessThan => compareToResult < 0,
            ComparisonOperator.LessThanOrEqual => compareToResult <= 0,

            _ => throw new InvalidOperationException("Unknown comparison operator")
        });
    }
}