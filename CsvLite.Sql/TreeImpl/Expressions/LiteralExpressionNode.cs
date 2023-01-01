using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
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

    public IPrimitiveDomain Domain { get; }
    public PrimitiveValue Value { get; }

    public LiteralExpressionNode(IPrimitiveDomain domain, PrimitiveValue value)
    {
        Domain = domain;
        Value = value;
    }

    public LiteralExpressionNode(string literal)
    {
        Domain = new StringDomain();
        Value = new StringValue(literal);
    }

    public LiteralExpressionNode(int literal)
    {
        Domain = new IntegerDomain();
        Value = new IntegerValue(literal);
    }

    public LiteralExpressionNode(bool literal)
    {
        Domain = new BooleanDomain();
        Value = new BooleanValue(literal);
    }

    public IDomain EvaluateDomain(IRelationContext context)
    {
        return Domain;
    }

    public IValue EvaluateValue(IRecordContext context)
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