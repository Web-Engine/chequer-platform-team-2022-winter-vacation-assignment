using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Tree.Expressions;

public interface IExpressionNode : INode
{
    IDomain EvaluateDomain(IRelationContext context);
    
    IValue EvaluateValue(IRecordContext context);
}