using CsvLite.IO.Csv;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ConcatRelationNode : IBinaryRelationNode
{
    public IRelationNode BaseRelationNode1 { get; }
    public IRelationNode BaseRelationNode2 { get; }

    public ConcatRelationNode(IRelationNode baseRelationNode1, IRelationNode baseRelationNode2)
    {
        BaseRelationNode1 = baseRelationNode1;
        BaseRelationNode2 = baseRelationNode2;
    }

    public IRelation Combine(IRelation baseRelation1, IRelation baseRelation2)
    {
        if (baseRelation1.Attributes.Count != baseRelation2.Attributes.Count)
            throw new Exception("Cannot concat(union) difference attribute size relations");

        return new DefaultRelation(
            baseRelation1.Attributes,
            baseRelation1.Records.Concat(baseRelation2.Records).ToList()
        );
    }

    public IRelation Evaluate(IRelationEvaluateContext context)
    {
        return context.Relation;
    }
}