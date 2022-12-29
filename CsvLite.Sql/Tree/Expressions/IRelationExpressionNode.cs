using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Expressions;

public interface IRelationExpressionNode : IExpressionNode
{
    IValue IExpressionNode.Evaluate(IRecordContext context) => Evaluate(context);

    new RelationValue Evaluate(IRecordContext context);
}