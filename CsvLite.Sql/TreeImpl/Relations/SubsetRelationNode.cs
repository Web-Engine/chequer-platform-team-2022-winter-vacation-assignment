using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class SubsetRelationNode : BaseInheritRelationNode
{
    private readonly IEnumerable<Identifier> _attributes;

    public SubsetRelationNode(IRelationNode relationNode, IEnumerable<Identifier> attributes) : base(relationNode)
    {
        _attributes = attributes;
    }

    protected override IRelation Evaluate(IRelationContext context)
    {
        if (context.Relation is not IWritableRelation writableRelation)
            throw new InvalidOperationException("Cannot create subset of non-writable relation");
        
        return new SubsetRelation(writableRelation, _attributes);
    }
}