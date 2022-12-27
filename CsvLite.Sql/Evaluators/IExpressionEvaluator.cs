using CsvLite.Models.Values;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.Evaluators;

public interface IExpressionEvaluator
{
    IValue Evaluate(IExpressionNode node);
}
