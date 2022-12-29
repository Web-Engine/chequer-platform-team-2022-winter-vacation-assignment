using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class AliasRelationNode : BaseInheritRelationNode
{
    private readonly Identifier _alias;

    public AliasRelationNode(IRelationNode baseRelationNode, Identifier alias) : base(baseRelationNode)
    {
        _alias = alias;
    }

    protected override IRelation Evaluate(IRelationContext context)
    {
        return new InheritRelation(
            context,
            
            attributeTransformer:
            attribute => new DefaultAttribute(
                _alias,
                attribute.Name
            ));
    }
}