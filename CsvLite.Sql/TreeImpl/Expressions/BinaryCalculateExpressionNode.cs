using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
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

    public IDomain EvaluateDomain(IRelationContext context)
    {
        var domain1 = ExpressionNode1.Value.EvaluateDomain(context);
        var domain2 = ExpressionNode2.Value.EvaluateDomain(context);

        return EvaluateDomain(domain1, domain2);
    }

    public IDomain EvaluateDomain(IDomain domain1, IDomain domain2)
    {
        switch (domain1, domain2)
        {
            case (IntegerDomain, IntegerDomain):
                return new IntegerDomain();

            case (BooleanDomain, BooleanDomain):
                return new IntegerDomain();

            case (StringDomain, _):
            case (_, StringDomain):
                return new StringDomain();
        }

        throw new InvalidOperationException($"Cannot calculate {domain1} and {domain2}");
    }

    public PrimitiveValue EvaluateValue(IRecordContext context)
    {
        var value1 = ExpressionNode1.Evaluate(context).AsPrimitive();
        var value2 = ExpressionNode2.Evaluate(context).AsPrimitive();

        return EvaluateValue(value1, value2);
    }

    public PrimitiveValue EvaluateValue(PrimitiveValue value1, PrimitiveValue value2)
    {
        return (value1, value2) switch
        {
            (NullValue, _) => throw new InvalidOperationException("Cannot calculate with DbNull"),
            (_, NullValue) => throw new InvalidOperationException("Cannot calculate with DbNull"),

            (BooleanValue booleanValue1, BooleanValue booleanValue2) =>
                EvaluateIntegerValue(booleanValue1.AsInteger(), booleanValue2.AsInteger()),

            (IntegerValue integerValue1, IntegerValue integerValue2) =>
                EvaluateIntegerValue(integerValue1, integerValue2),

            (StringValue stringValue1, _) => EvaluateStringValue(stringValue1, value2.AsString()),
            (_, StringValue stringValue2) => EvaluateStringValue(value1.AsString(), stringValue2),

            (_, _) => throw new InvalidOperationException("Cannot calculate")
        };
    }

    private IntegerValue EvaluateIntegerValue(IntegerValue value1, IntegerValue value2)
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

    private StringValue EvaluateStringValue(StringValue value1, StringValue value2)
    {
        return new StringValue(_operator switch
        {
            BinaryCalculateOperator.Addition => value1.Value + value2.Value,

            _ => throw new InvalidOperationException($"Invalid string operator {_operator.ToString()}")
        });
    }
}