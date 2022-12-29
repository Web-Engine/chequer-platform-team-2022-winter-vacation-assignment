using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Expressions;

public interface ITupleExpressionNode : IExpressionNode
{
    IValue IExpressionNode.Evaluate(IRecordContext context) => Evaluate(context);

    new TupleValue Evaluate(IRecordContext context);
}