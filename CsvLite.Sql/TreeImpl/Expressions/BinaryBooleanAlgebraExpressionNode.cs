using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class BinaryBooleanAlgebraExpressionNode : IPrimitiveExpressionNode
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

    public BinaryBooleanAlgebraOperator Operator { get; }

    public enum BinaryBooleanAlgebraOperator
    {
        And,
        Or
    }

    public BinaryBooleanAlgebraExpressionNode(IExpressionNode expressionNode1, BinaryBooleanAlgebraOperator @operator,
        IExpressionNode expressionNode2)
    {
        ExpressionNode1 = expressionNode1.ToNodeValue();
        Operator = @operator;
        ExpressionNode2 = expressionNode2.ToNodeValue();
    }

    PrimitiveValue IPrimitiveExpressionNode.EvaluateValue(IRecordContext context) => Evaluate(context);


    public IDomain EvaluateDomain(IRelationContext context)
    {
        return new BooleanDomain();
    }

    public BooleanValue Evaluate(IRecordContext context)
    {
        var value1 = ExpressionNode1.Evaluate(context);
        var value2 = ExpressionNode2.Evaluate(context);

        return Evaluate(value1, value2);
    }

    public BooleanValue Evaluate(IValue value1, IValue value2)
    {
        var boolean1 = value1.AsBoolean();
        var boolean2 = value2.AsBoolean();

        return Operator switch
        {
            BinaryBooleanAlgebraOperator.And => new BooleanValue(boolean1.Value && boolean2.Value),
            BinaryBooleanAlgebraOperator.Or => new BooleanValue(boolean1.Value || boolean2.Value),

            _ => throw new InvalidOperationException("Invalid operator")
        };
    }
}