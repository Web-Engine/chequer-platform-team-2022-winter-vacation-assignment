using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.RelationContexts;
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

    protected override IRelationContext Resolve(IRootContext rootContext)
    {
        var baseContext = base.Resolve(rootContext);

        return new NamedRelationContext(
            rootContext,
            _alias,
            baseContext.Relation
        );
    }
}