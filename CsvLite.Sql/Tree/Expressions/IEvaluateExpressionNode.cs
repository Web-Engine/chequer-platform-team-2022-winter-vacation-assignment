using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Expressions;

public interface IEvaluateExpressionNode : IExpressionNode
{
    IValue Evaluate(IExpressionEvaluateContext context);
}
