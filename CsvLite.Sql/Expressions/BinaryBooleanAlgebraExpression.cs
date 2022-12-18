using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.Expressions;

public class BinaryBooleanAlgebraExpression : IExpression
{
    private readonly IExpression _expression1;
    private readonly IExpression _expression2;

    private readonly BinaryBooleanAlgebraOperator _operator;
    
    public enum BinaryBooleanAlgebraOperator
    {
        And,
        Or,
    }

    public BinaryBooleanAlgebraExpression(IExpression expression1, BinaryBooleanAlgebraOperator @operator, IExpression expression2)
    {
        _expression1 = expression1;
        _operator = @operator;
        _expression2 = expression2;
    }

    public object? Evaluate(IAttribute[] attributes, ITuple tuple)
    {
        var value1 = ValueUtility.ToBoolean(_expression1.Evaluate(attributes, tuple));

        var value2 = ValueUtility.ToBoolean(_expression1.Evaluate(attributes, tuple));

        switch (_operator)
        {
            case BinaryBooleanAlgebraOperator.And:
                return value1 && value2 ? 1 : 0;
                
            case BinaryBooleanAlgebraOperator.Or:
                return value1 || value2 ? 1 : 0;
            
            default:
                throw new InvalidOperationException("Invalid operator");
        }
    }
}