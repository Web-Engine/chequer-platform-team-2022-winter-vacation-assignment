using CsvLite.Models.Relations;
using CsvLite.Models.Schemas;
using CsvLite.Sql.Evaluators;
using CsvLite.Sql.Models.Results;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.Actions;

public class SelectAction : IRelationAction
{
    private readonly IRelationNode _relationNode;

    public SelectAction(IRelationNode relationNode)
    {
        _relationNode = relationNode;
    }

    public IRelationResult Execute(IRelationProvider provider)
    {
        var evaluator = new RelationEvaluator(provider);
        var relation = evaluator.Evaluate(_relationNode);

        return new RelationResult(relation);
    }
}