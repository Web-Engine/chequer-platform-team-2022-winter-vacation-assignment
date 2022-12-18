using CsvLite.Models.Relations;
using CsvLite.Models.Schemas;
using CsvLite.Sql.Results;

namespace CsvLite.Sql.Actions;

public class UnionAction : IRelationAction
{
    private readonly IRelationAction _previousAction;
    private readonly IRelationAction _nextRelation;

    public UnionAction(IRelationAction previousAction, IRelationAction nextRelation)
    {
        _previousAction = previousAction;
        _nextRelation = nextRelation;
    }

    public IRelationResult Execute(ISchema schema)
    {
        var previousRelationResult = _previousAction.Execute(schema);
        var nextRelationResult = _nextRelation.Execute(schema);

        return new RelationResult(
            new UnionRelation(previousRelationResult.Relation, nextRelationResult.Relation)
        );
    }
}
