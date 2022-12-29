using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.RelationContexts;

public class InheritRelationContext : IRelationContext
{
    IContext? IContext.Parent => Inherit.Parent;

    public IRelationContext Inherit { get; }

    public IRelation Relation { get; }

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers => Inherit.AttributeIdentifiers;

    public InheritRelationContext(IRelationContext inherit, IRelation relation)
    {
        Inherit = inherit;
        Relation = relation;
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Inherit.GetPhysicalContext(identifier);
    }
}