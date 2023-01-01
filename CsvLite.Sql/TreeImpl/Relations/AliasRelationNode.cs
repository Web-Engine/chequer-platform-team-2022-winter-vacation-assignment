using CsvLite.Models.Identifiers;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class AliasRelationNode : BaseInheritRelationNode, INamedRelationNode
{
    public Identifier Identifier { get; }

    public AliasRelationNode(IRelationNode baseRelationNode, Identifier alias) : base(baseRelationNode)
    {
        Identifier = alias;
    }
}