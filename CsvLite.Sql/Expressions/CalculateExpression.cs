using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.Expressions;

public class BinaryCalculateExpression : IExpression
{
    private readonly IExpression _expression1;
    private readonly IExpression _expression2;

    private readonly BinaryCalculateOperator _operator;
    
    public enum BinaryCalculateOperator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Modulus
    }

    public BinaryCalculateExpression(IExpression expression1, BinaryCalculateOperator @operator, IExpression expression2)
    {
        _expression1 = expression1;
        _operator = @operator;
        _expression2 = expression2;
    }

    public object? Evaluate(IAttribute[] attributes, ITuple tuple)
    {
        var value1 = _expression1.Evaluate(attributes, tuple);
        var value2 = _expression1.Evaluate(attributes, tuple);

        if (value1 is double double1)
            return EvaluateDoubleInternal(double1, value2);
        
        if (value2 is double double2)
            return EvaluateDoubleInternal(value1, double2);

        var long1 = ValueUtility.ToLong(_expression1);
        var long2 = ValueUtility.ToLong(_expression2);

        return EvaluateLongInternal(long1, long2);
    }

    private object? EvaluateDoubleInternal(double value1, object? value2)
    {
        if (value2 is double double2)
            return EvaluateDoubleInternal(value1, double2);

        return EvaluateDoubleInternal(value1, ValueUtility.ToDouble(value2));
    }

    private object? EvaluateDoubleInternal(object? value1, double value2)
    {
        if (value1 is double double1)
            return EvaluateDoubleInternal(double1, value2);
        
        return EvaluateDoubleInternal(ValueUtility.ToDouble(value1), value2);
    }

    private object? EvaluateDoubleInternal(double value1, double value2)
    {
        return _operator switch
        {
            BinaryCalculateOperator.Addition => value1 + value2,
            BinaryCalculateOperator.Subtraction => value1 - value2,
            BinaryCalculateOperator.Multiplication => value1 * value2,
            BinaryCalculateOperator.Division => value1 / value2,
            BinaryCalculateOperator.Modulus => value1 % value2,
            
            _ => throw new InvalidOperationException($"Invalid operator {_operator.ToString()}")
        };
    }
    
    private object? EvaluateLongInternal(long value1, long value2)
    {
        return _operator switch
        {
            BinaryCalculateOperator.Addition => value1 + value2,
            BinaryCalculateOperator.Subtraction => value1 - value2,
            BinaryCalculateOperator.Multiplication => value1 * value2,
            BinaryCalculateOperator.Division => value1 / value2,
            BinaryCalculateOperator.Modulus => value1 % value2,
            
            _ => throw new InvalidOperationException($"Invalid operator {_operator.ToString()}")
        };
    }
}