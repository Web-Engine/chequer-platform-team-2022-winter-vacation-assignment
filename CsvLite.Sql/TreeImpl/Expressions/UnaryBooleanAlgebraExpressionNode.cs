using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class UnaryBooleanAlgebraExpressionNode : IPrimitiveExpressionNode
{
    public enum UnaryBooleanAlgebraOperator
    {
        Not,
    }

    public IEnumerable<INodeValue> Children
    {
        get { yield return ExpressionNode; }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    private readonly UnaryBooleanAlgebraOperator _operator;

    public UnaryBooleanAlgebraExpressionNode(UnaryBooleanAlgebraOperator @operator, IExpressionNode expressionNode)
    {
        ExpressionNode = expressionNode.ToNodeValue();
        _operator = @operator;
    }

    PrimitiveValue IPrimitiveExpressionNode.EvaluateValue(IRecordContext context) => Evaluate(context);


    public IDomain EvaluateDomain(IRelationContext context)
    {
        return new BooleanDomain();
    }

    public BooleanValue Evaluate(IRecordContext context)
    {
        var value = ExpressionNode.Evaluate(context);

        return Evaluate(value);
    }

    public BooleanValue Evaluate(IValue value)
    {
        var boolean = value.AsBoolean().Value;

        return _operator switch
        {
            UnaryBooleanAlgebraOperator.Not => new BooleanValue(!boolean),

            _ => throw new InvalidOperationException("Invalid operator")
        };
    }
}