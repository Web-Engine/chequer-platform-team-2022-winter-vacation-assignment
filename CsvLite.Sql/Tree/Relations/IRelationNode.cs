using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Relations;

public interface IRelationNode : INode
{
    IRelationContext Evaluate(IRootContext context);
}
