using CsvLite.Models.Values;
using CsvLite.Models.Values.Primitives;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions.Functions;

public class CountExpressionNode : IExpressionNode, IImplicitAggregateNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return ExpressionNode; }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    private readonly bool _distinct;

    public CountExpressionNode(IExpressionNode expressionNode, bool distinct)
    {
        ExpressionNode = expressionNode.ToNodeValue();
        _distinct = distinct;
    }

    public IValue Evaluate(IRecordContext context)
    {
        var value = ExpressionNode.Evaluate(context);

        if (value is TupleValue tupleValue)
            value = tupleValue.First();

        if (value is not AggregateValue aggregateValue)
            return new IntegerValue(1);

        IEnumerable<IValue> values = aggregateValue;

        if (_distinct)
            values = aggregateValue.Distinct();

        var count = values.Count();

        return new IntegerValue(count);
    }
}