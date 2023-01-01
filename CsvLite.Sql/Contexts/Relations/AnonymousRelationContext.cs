using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public class AnonymousRelationContext : BaseRelationContext
{
    public override IEnumerable<QualifiedIdentifier> AttributeIdentifiers
    {
        get
        {
            return Relation.Attributes.Select(
                attribute => new QualifiedIdentifier(Identifier.Empty, attribute.Identifier)
            );
        }
    }

    public AnonymousRelationContext(IContext parent, IRelation relation) : base(parent, relation)
    {
    }
}