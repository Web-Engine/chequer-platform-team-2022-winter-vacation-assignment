using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Evaluators;

namespace CsvLite.Sql.Contexts;

public sealed class RelationEvaluateContext : IRelationEvaluateContext
{
    public IRelationProvider RelationProvider { get; }

    public IRelation Relation { get; }

    public IExpressionEvaluateContext? Parent { get; }

    public RelationEvaluateContext(IRelationProvider relationProvider, IRelation relation) : this(relationProvider, relation,
        null)
    {
    }

    internal RelationEvaluateContext(IRelationProvider relationProvider, IRelation relation, IExpressionEvaluateContext? parent)
    {
        RelationProvider = relationProvider;
        Relation = relation;
        Parent = parent;
    }

    public RelationEvaluateContext CreateSubContext(IRelation relation)
    {
        return new RelationEvaluateContext(RelationProvider, relation);
    }

    public IExpressionEvaluateContext CreateExpressionEvaluateContext(IRecord record)
    {
        return new ExpressionEvaluateContext(this, record);
    }
}