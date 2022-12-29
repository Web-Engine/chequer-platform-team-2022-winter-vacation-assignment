using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class TupleExpressionNode : ITupleExpressionNode
{
    public IEnumerable<INodeValue> Children => ExpressionNodes;

    public List<NodeValue<IExpressionNode>> ExpressionNodes { get; }

    public TupleExpressionNode(IEnumerable<IExpressionNode> expressionNodes)
    {
        ExpressionNodes = expressionNodes.Select(node => node.ToNodeValue()).ToList();
    }

    public TupleValue Evaluate(IRecordContext context)
    {
        return new TupleValue(
            ExpressionNodes.Select(node => node.Evaluate(context))
        );
    }
}