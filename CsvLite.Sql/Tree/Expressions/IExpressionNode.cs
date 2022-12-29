using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Expressions;

public interface IExpressionNode : INode
{
    IValue Evaluate(IRecordContext recordContext);
}