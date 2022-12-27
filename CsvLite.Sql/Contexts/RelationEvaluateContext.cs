using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Evaluators;

namespace CsvLite.Sql.Contexts;

public sealed class RelationEvaluateContext : IRelationEvaluateContext
{
    public IPhysicalRelationProvider PhysicalRelationProvider { get; }

    public IRelation Relation { get; }

    public IExpressionEvaluateContext? Parent { get; }

    public RelationEvaluateContext(IPhysicalRelationProvider physicalRelationProvider, IRelation relation) : this(physicalRelationProvider, relation,
        null)
    {
    }

    internal RelationEvaluateContext(IPhysicalRelationProvider physicalRelationProvider, IRelation relation, IExpressionEvaluateContext? parent)
    {
        PhysicalRelationProvider = physicalRelationProvider;
        Relation = relation;
        Parent = parent;
    }

    public RelationEvaluateContext CreateSubContext(IRelation relation)
    {
        return new RelationEvaluateContext(PhysicalRelationProvider, relation);
    }

    public IExpressionEvaluateContext CreateExpressionEvaluateContext(IRecord record)
    {
        return new ExpressionEvaluateContext(this, record);
    }
}