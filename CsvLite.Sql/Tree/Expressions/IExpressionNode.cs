using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;

namespace CsvLite.Sql.Tree.Expressions;

public interface IExpressionNode : INode
{
    IValue Evaluate(IRecordContext recordContext);
}