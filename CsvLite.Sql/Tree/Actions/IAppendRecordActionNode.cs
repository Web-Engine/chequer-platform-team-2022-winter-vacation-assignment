using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.Tree.Actions;

public interface IAppendRecordActionNode : IActionNode
{
    IRelationNode TargetRelationNode { get; }

    IRelationNode ValueRelationNode { get; }
}