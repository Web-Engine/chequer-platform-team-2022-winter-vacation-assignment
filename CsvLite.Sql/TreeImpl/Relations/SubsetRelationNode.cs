using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.RelationContexts;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.TreeImpl.Attributes;
using CsvLite.Sql.Utilities;
using CsvLite.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class SubsetRelationNode : BaseInheritRelationNode
{
    public List<NodeValue<IAttributeReferenceNode>> ReferenceNodes { get; }

    public SubsetRelationNode(
        IRelationNode relationNode,
        IEnumerable<IAttributeReferenceNode> referenceNodes
    ) : base(relationNode)
    {
        ReferenceNodes = referenceNodes.Select(node => node.ToNodeValue()).ToList();
    }

    protected override IRelationContext Evaluate(IRelationContext context)
    {
        if (context.Relation is not IWritableRelation writableRelation)
            throw new InvalidOperationException("Cannot create subset of non-writable relation");

        var indexes = ReferenceNodes
            .SelectMany(node => node.Value.GetAttributeIndexes(context, out _))
            .WithIndex()
            .ToList();

        var relation = new SubsetRelation(writableRelation, indexes);

        return new InheritRelationContext(context, relation);
    }
}