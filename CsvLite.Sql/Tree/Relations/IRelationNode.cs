using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Relations;

public interface IRelationNode : INode
{
    IRelation Evaluate(IRootContext context);
}
