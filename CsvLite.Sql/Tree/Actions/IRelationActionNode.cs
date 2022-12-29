using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.Tree.Actions;

public interface IRelationActionNode : IActionNode
{
    IRelationNode RelationNode { get; }
}