using CsvLite.IO.Csv;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ConcatRelationNode : BaseBinaryRelationNode
{
    public ConcatRelationNode(IRelationNode relationNode1, IRelationNode relationNode2) : base(relationNode1,
        relationNode2)
    {
    }

    protected override IRelationContext Combine(IRelationContext context1, IRelationContext context2)
    {
        if (context1.Attributes.Count != context2.Attributes.Count)
            throw new Exception("Cannot concat(union) difference attribute size relations");

        var relation = new InheritRelation(
            context1.Relation,
            records: context1.Records.Concat(context2.Records)
        );

        return new InheritRelationContext(context1, relation);
    }
}