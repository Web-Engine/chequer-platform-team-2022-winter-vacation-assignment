using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;

namespace CsvLite.Sql.Tree.Expressions;

public interface IPrimitiveExpressionNode : IExpressionNode
{
    IValue IExpressionNode.Evaluate(IRecordContext context) => Evaluate(context);

    new PrimitiveValue Evaluate(IRecordContext context);
}