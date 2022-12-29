using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class CrossJoinRelationNode : BaseBinaryRelationNode
{
    public CrossJoinRelationNode(
        IRelationNode baseRelationNode1,
        IRelationNode baseRelationNode2
    ) : base(baseRelationNode1, baseRelationNode2)
    {
    }

    protected override IRelation Evaluate(IRelationContext context1, IRelationContext context2)
    {
        return new ComplexRelation(context1.Relation, context2.Relation);
    }
}
