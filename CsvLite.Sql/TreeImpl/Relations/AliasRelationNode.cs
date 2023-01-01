using CsvLite.Models.Identifiers;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class AliasRelationNode : BaseInheritRelationNode
{
    private readonly Identifier _alias;

    public AliasRelationNode(IRelationNode baseRelationNode, Identifier alias) : base(baseRelationNode)
    {
        _alias = alias;
    }

    protected override IRelationContext Resolve(IContext rootContext)
    {
        var baseContext = base.Resolve(rootContext);

        return new NamedRelationContext(
            rootContext,
            _alias,
            baseContext.Relation
        );
    }
}