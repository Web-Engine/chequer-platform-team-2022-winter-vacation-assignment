using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Models.Records;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class LiteralExpressionNode : IExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public PrimitiveValue Value { get; }

    public LiteralExpressionNode(PrimitiveValue value)
    {
        Value = value;
    }

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

    public IValue Evaluate(IRecordContext context)
    {
        if (context.Record is AggregateRecord aggregateRecord)
        {
            return new AggregateValue(
                aggregateRecord.Select(x => Value),
                true
            );
        }

        return Value;
    }
}