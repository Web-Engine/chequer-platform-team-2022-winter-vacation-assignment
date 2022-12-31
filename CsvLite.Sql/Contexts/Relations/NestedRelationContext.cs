using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public class NestedRelationContext : IRelationContext
{
    IContext? IContext.Parent => Parent;

    public IRelationContext Parent { get; }

    public IRelationContext Child { get; }

    public IRelation Relation => Child.Relation;

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers => Child.AttributeIdentifiers;

    public NestedRelationContext(IRelationContext parent, IRelationContext child)
    {
        Parent = parent;
        Child = child;
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Parent.GetPhysicalContext(identifier);
    }
}