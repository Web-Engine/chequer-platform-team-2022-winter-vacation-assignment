using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.RelationContexts;

public class AnonymousRelationContext : IRelationContext
{
    IContext? IContext.Parent => Parent;

    public IRootContext Parent { get; }

    public IRelation Relation { get; }

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers { get; }

    public AnonymousRelationContext(IRootContext parent, IRelation relation)
    {
        Parent = parent;
        Relation = relation;

        AttributeIdentifiers = Relation.Attributes
            .Select(x => new QualifiedIdentifier(Identifier.Empty, x.Name))
            .ToList();
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Parent.GetPhysicalContext(identifier);
    }
}