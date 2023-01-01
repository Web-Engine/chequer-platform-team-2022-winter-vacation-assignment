using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions.Functions;

public class MinExpressionNode : IExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return ExpressionNode; }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    public MinExpressionNode(IExpressionNode expressionNode)
    {
        ExpressionNode = expressionNode.ToNodeValue();
    }

    public IDomain EvaluateDomain(IRelationContext context)
    {
        return new IntegerDomain();
    }

    public IValue EvaluateValue(IRecordContext context)
    {
        var value = ExpressionNode.Evaluate(context);

        if (value is not AggregateValue aggregateValue)
            return value;

        var min = aggregateValue.Min();
        if (min is null)
            throw new InvalidOperationException();

        return min;
    }
}