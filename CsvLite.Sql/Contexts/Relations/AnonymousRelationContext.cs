using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public class AnonymousRelationContext : IRelationContext
{
    public IContext Parent { get; }

    public IRelation Relation { get; }

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers { get; }

    public AnonymousRelationContext(IContext parent, IRelation relation)
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