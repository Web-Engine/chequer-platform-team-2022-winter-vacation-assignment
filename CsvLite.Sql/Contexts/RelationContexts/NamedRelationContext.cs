using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.RelationContexts;

public class NamedRelationContext : IRelationContext
{
    IContext? IContext.Parent => Parent;

    public IRootContext Parent { get; }

    public IRelation Relation { get; }

    public Identifier Name { get; }

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers { get; }

    public NamedRelationContext(IRootContext parent, Identifier name, IRelation relation)
    {
        Parent = parent;
        Parent = parent;
        Relation = relation;
        Name = name;

        AttributeIdentifiers = Relation.Attributes
            .Select(attribute => new QualifiedIdentifier(Name, attribute.Name))
            .ToList();
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Parent.GetPhysicalContext(identifier);
    }
}