using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public class NamedRelationContext : BaseRelationContext
{
    public Identifier RelationIdentifier { get; }

    public override IEnumerable<QualifiedIdentifier> AttributeIdentifiers
    {
        get
        {
            return Relation.Attributes.Select(
                attribute => new QualifiedIdentifier(RelationIdentifier, attribute.Identifier)
            );
        }
    }

    public NamedRelationContext(IContext parent, Identifier relationIdentifier, IRelation relation) : base(parent,
        relation)
    {
        RelationIdentifier = relationIdentifier;
    }
}