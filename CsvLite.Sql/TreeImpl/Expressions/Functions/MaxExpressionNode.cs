using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions.Functions;

public class MaxExpressionNode : IExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return ExpressionNode; }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    public MaxExpressionNode(IExpressionNode expressionNode)
    {
        ExpressionNode = expressionNode.ToNodeValue();
    }

    public IValue Evaluate(IRecordContext context)
    {
        var value = ExpressionNode.Evaluate(context);

        if (value is not AggregateValue aggregateValue)
            return value;

        var max = aggregateValue.Max();
        if (max is null)
            throw new InvalidOperationException();

        return max;
    }
}