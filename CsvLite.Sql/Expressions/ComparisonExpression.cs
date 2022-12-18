using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Expressions;

public class ComparisonExpression : IExpression
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

    private readonly IExpression _expression1;
    private readonly IExpression _expression2;

    private readonly ComparisonOperator _operator;

    public ComparisonExpression(IExpression expression1, ComparisonOperator @operator, IExpression expression2)
    {
        _expression1 = expression1;
        _operator = @operator;
        _expression2 = expression2;
    }

    public object? Evaluate(IAttribute[] attributes, ITuple tuple)
    {
        var value1 = _expression1.Evaluate(attributes, tuple);
        var value2 = _expression2.Evaluate(attributes, tuple);

        if (value1 == null && value2 == null)
            return true;

        if (value1 == null)
            return false;
        
        if (value2 == null)
            return false;

        if (value1 is IComparable comparable1)
        {
            return Compare(comparable1, value2);
        }

        if (value2 is IComparable comparable2)
        {
            return !Compare(comparable2, value1);
        }

        throw new IOException("Cannot compare");
    }

    private bool Compare(IComparable comparable, object value)
    {
        var result = comparable.CompareTo(value);

        return _operator switch
        {
            ComparisonOperator.Equal => result == 0,
            ComparisonOperator.NotEqual => result != 0,
            ComparisonOperator.GreaterThan => result > 0,
            ComparisonOperator.GreaterThanOrEqual => result >= 0,
            ComparisonOperator.LessThan => result < 0,
            ComparisonOperator.LessThanOrEqual => result <= 0,
            
            _ => throw new InvalidOperationException($"Unknown operator {_operator.ToString()}")
        };
    }
}