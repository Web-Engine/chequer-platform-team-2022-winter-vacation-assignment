using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class BinaryBooleanAlgebraExpressionNode : IEvaluateExpressionNode
{
    private readonly IExpressionNode _expression1;
    private readonly IExpressionNode _expression2;

    private readonly BinaryBooleanAlgebraOperator _operator;
    
    public enum BinaryBooleanAlgebraOperator
    {
        And,
        Or
    }

    public BinaryBooleanAlgebraExpressionNode(IExpressionNode expression1, BinaryBooleanAlgebraOperator @operator, IExpressionNode expression2)
    {
        _expression1 = expression1;
        _operator = @operator;
        _expression2 = expression2;
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        
        var boolean1 = evaluator.Evaluate(_expression1).AsBoolean();
        var boolean2 = evaluator.Evaluate(_expression2).AsBoolean();

        return _operator switch
        {
            BinaryBooleanAlgebraOperator.And => new BooleanValue(boolean1.Value && boolean2.Value),
            BinaryBooleanAlgebraOperator.Or => new BooleanValue(boolean1.Value || boolean2.Value),
            
            _ => throw new InvalidOperationException("Invalid operator")
        };
    }
}