using CsvLite.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.Evaluators;

public interface IRelationEvaluator
{
    IRelation Evaluate(IRelationNode node);
}
