using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Evaluators;

namespace CsvLite.Sql.Contexts;

public sealed class ExpressionEvaluateContext : IExpressionEvaluateContext
{
    public RelationEvaluateContext Parent { get; }

    public ExpressionEvaluateContext(RelationEvaluateContext parent, IRecord record)
    {
        Parent = parent;
        Record = record;
    }

    IRelationEvaluateContext IExpressionEvaluateContext.Parent => Parent;

    public IRelation Relation => Parent.Relation;

    public IRecord Record { get; }

    public IRelationEvaluator CreateRelationEvaluator()
    {
        return new RelationEvaluator(
            new RelationEvaluateContext(
                Parent.PhysicalRelationProvider,
                Parent.Relation,
                this
            )
        );
    }

    public IExpressionEvaluator CreateExpressionEvaluator()
    {
        return new ExpressionEvaluator(
            this
        );
    }
}