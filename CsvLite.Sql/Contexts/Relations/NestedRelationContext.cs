using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts.Records;

namespace CsvLite.Sql.Contexts.Relations;

public class NestedRelationContext : IRelationContext
{
    IContext IRelationContext.Parent => Parent;

    public IRecordContext Parent { get; }

    public IRelationContext Child { get; }

    public IRelation Relation => Child.Relation;

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers => Child.AttributeIdentifiers;

    public NestedRelationContext(IRecordContext parent, IRelationContext child)
    {
        Parent = parent;
        Child = child;
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Parent.GetPhysicalContext(identifier);
    }
}