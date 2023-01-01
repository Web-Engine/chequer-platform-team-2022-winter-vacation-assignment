using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public class InheritRelationContext : BaseRelationContext
{
    public IRelationContext Inherit { get; }

    public override IEnumerable<QualifiedIdentifier> AttributeIdentifiers => Inherit.AttributeIdentifiers;

    public InheritRelationContext(IRelationContext inherit, IRelation relation) : base(inherit.Parent, relation)
    {
        Inherit = inherit;
    }
}