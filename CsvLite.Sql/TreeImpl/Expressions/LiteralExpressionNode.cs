using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class LiteralExpressionNode : IEvaluateExpressionNode
{
    public IValue Value { get; }

    public LiteralExpressionNode(string literal)
    {
        Value = new StringValue(literal);
    }

    public LiteralExpressionNode(int literal)
    {
        Value = new IntegerValue(literal);
    }

    public LiteralExpressionNode(bool literal)
    {
        Value = new BooleanValue(literal);
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        return Value;
    }
}