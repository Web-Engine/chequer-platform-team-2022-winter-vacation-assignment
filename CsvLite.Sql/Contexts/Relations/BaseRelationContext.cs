using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public abstract class BaseRelationContext : IRelationContext
{
    public IContext Parent { get; }

    public IRelation Relation { get; }

    public abstract IEnumerable<QualifiedIdentifier> AttributeIdentifiers { get; }

    public BaseRelationContext(IContext parent, IRelation relation)
    {
        Parent = parent;
        Relation = relation;
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Parent.GetPhysicalContext(identifier);
    }
}