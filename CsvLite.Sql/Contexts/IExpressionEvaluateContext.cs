using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Evaluators;

namespace CsvLite.Sql.Contexts;

public interface IExpressionEvaluateContext
{
    IRelationEvaluateContext Parent { get; }
    
    IRelation Relation { get; }

    IRecord Record { get; }

    IRelationEvaluator CreateRelationEvaluator();

    IExpressionEvaluator CreateExpressionEvaluator();
}
