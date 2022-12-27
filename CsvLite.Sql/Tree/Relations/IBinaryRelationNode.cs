using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Relations;

public interface IBinaryRelationNode : IRelationNode
{
    IRelationNode BaseRelationNode1 { get; }

    IRelationNode BaseRelationNode2 { get; }

    IRelation Combine(IRelation baseRelation1, IRelation baseRelation2);
    
    IRelation Evaluate(IRelationEvaluateContext context);
}