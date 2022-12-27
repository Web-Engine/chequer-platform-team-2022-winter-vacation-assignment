using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Evaluators;

namespace CsvLite.Sql.Contexts;

public interface IRelationEvaluateContext
{
    IExpressionEvaluateContext? Parent { get; }
    
    IRelation Relation { get; }

    IExpressionEvaluateContext CreateExpressionEvaluateContext(IRecord record);
}
