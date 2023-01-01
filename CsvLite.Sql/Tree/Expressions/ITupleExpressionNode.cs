using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Tree.Expressions;

public interface ITupleExpressionNode : IExpressionNode
{
    IValue IExpressionNode.EvaluateValue(IRecordContext context) => EvaluateValue(context);

    IDomain IExpressionNode.EvaluateDomain(IRelationContext context) => EvaluateDomain(context);

    new TupleValue EvaluateValue(IRecordContext context);

    new TupleDomain EvaluateDomain(IRelationContext context);
}