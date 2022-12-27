using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class UnaryBooleanAlgebraExpressionNode : IEvaluateExpressionNode
{
    private readonly IExpressionNode _expressionNode;

    private readonly UnaryBooleanAlgebraOperator _operator;
    
    public enum UnaryBooleanAlgebraOperator
    {
        Not,
    }

    public UnaryBooleanAlgebraExpressionNode(UnaryBooleanAlgebraOperator @operator, IExpressionNode expressionNode)
    {
        _operator = @operator;
        _expressionNode = expressionNode;
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateExpressionEvaluator();
        
        var value = evaluator.Evaluate(_expressionNode).AsBoolean().Value;

        return _operator switch
        {
            UnaryBooleanAlgebraOperator.Not => new BooleanValue(!value),
            
            _ => throw new InvalidOperationException("Invalid operator")
        };
    }
}