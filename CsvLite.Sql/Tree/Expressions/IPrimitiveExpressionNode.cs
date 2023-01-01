using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;

namespace CsvLite.Sql.Tree.Expressions;

public interface IPrimitiveExpressionNode : IExpressionNode
{
    IValue IExpressionNode.EvaluateValue(IRecordContext context) => EvaluateValue(context);

    new PrimitiveValue EvaluateValue(IRecordContext context);
}