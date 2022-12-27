using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class CrossJoinRelationNode : IBinaryRelationNode
{
    public IRelationNode BaseRelationNode1 { get; }
    public IRelationNode BaseRelationNode2 { get; }

    public CrossJoinRelationNode(IRelationNode baseRelationNode1, IRelationNode baseRelationNode2)
    {
        BaseRelationNode1 = baseRelationNode1;
        BaseRelationNode2 = baseRelationNode2;
    }

    public IRelation Combine(IRelation baseRelation1, IRelation baseRelation2)
    {
        return new ComplexRelation(baseRelation1, baseRelation2);
    }

    public IRelation Evaluate(IRelationEvaluateContext context)
    {
        return context.Relation;
    }
}