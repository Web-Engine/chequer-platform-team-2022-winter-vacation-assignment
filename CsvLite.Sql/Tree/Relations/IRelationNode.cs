using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Tree.Relations;

public interface IRelationNode : INode
{
    IRelationContext Evaluate(IContext context);
}
