using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class ComparisonExpressionNode : IEvaluateExpressionNode
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

    private readonly IExpressionNode _expression1;
    private readonly IExpressionNode _expression2;

    private readonly ComparisonOperator _operator;

    public ComparisonExpressionNode(IExpressionNode expression1, ComparisonOperator @operator, IExpressionNode expression2)
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
            (NullValue, NullValue) => EvaluateNull(),
            (BooleanValue bool1, BooleanValue bool2) => EvaluateBoolean(bool1, bool2),
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
            ComparisonOperator.NotEqual => false,
            
            _ => throw new InvalidOperationException("Cannot calculate with DbNull")
        });
    }

    private BooleanValue EvaluateBoolean(BooleanValue boolean1, BooleanValue boolean2)
    {
        return new BooleanValue(_operator switch
        {
            ComparisonOperator.Equal => boolean1.Value == boolean2.Value,
            ComparisonOperator.NotEqual => boolean1.Value != boolean2.Value,

            _ => throw new InvalidOperationException("Cannot calculate with DbBoolean")
        });
    }

    private BooleanValue EvaluateInteger(IntegerValue integer1, IntegerValue integer2)
    {
        return new BooleanValue(_operator switch
        {
            ComparisonOperator.Equal => integer1.Value == integer2.Value,
            ComparisonOperator.NotEqual => integer1.Value != integer2.Value,
            
            ComparisonOperator.GreaterThan => integer1.Value > integer2.Value,
            ComparisonOperator.GreaterThanOrEqual => integer1.Value >= integer2.Value,
            
            ComparisonOperator.LessThan => integer1.Value < integer2.Value,
            ComparisonOperator.LessThanOrEqual => integer1.Value <= integer2.Value,

            _ => throw new InvalidOperationException("Cannot calculate with DbBoolean")
        });
    }

    private BooleanValue EvaluateString(StringValue str1, StringValue str2)
    {
        return new BooleanValue(_operator switch
        {
            ComparisonOperator.Equal => str1.Value == str2.Value,
            ComparisonOperator.NotEqual => str1.Value != str2.Value,

            _ => throw new InvalidOperationException("Cannot calculate with DbBoolean")
        });
    }
}