using CsvLite.IO.Csv;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ConcatRelationNode : BaseBinaryRelationNode
{
    public ConcatRelationNode(IRelationNode relationNode1, IRelationNode relationNode2) : base(relationNode1,
        relationNode2)
    {
    }

    protected override IRelation Evaluate(IRelationContext context1, IRelationContext context2)
    {
        var relation1 = context1.Relation;
        var relation2 = context2.Relation;

        if (relation1.Attributes.Count != relation2.Attributes.Count)
            throw new Exception("Cannot concat(union) difference attribute size relations");

        return new InheritRelation(
            relation1,
            records: relation1.Records.Concat(relation2.Records)
        );
    }
}