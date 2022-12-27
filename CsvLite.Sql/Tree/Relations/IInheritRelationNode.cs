using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Relations;

public interface IInheritRelationNode : IRelationNode
{
    IRelationNode? BaseRelationNode { get; }
    
    IRelation Evaluate(IRelationEvaluateContext context);

}