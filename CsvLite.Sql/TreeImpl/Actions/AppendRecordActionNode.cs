using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Results.Actions;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Actions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Actions;

public class AppendRecordActionNode : IActionNode
{
    public IEnumerable<INodeValue> Children
    {
        get
        {
            yield return TargetRelationNode;
            yield return ValueRelationNode;
        }
    }

    public NodeValue<IRelationNode> TargetRelationNode { get; }
    public NodeValue<IRelationNode> ValueRelationNode { get; }

    public AppendRecordActionNode(IRelationNode targetRelation, IRelationNode valueRelation)
    {
        TargetRelationNode = targetRelation.ToNodeValue();
        ValueRelationNode = valueRelation.ToNodeValue();
    }

    public IActionResult Execute(IContext context)
    {
        var targetRelationContext = TargetRelationNode.Value.Evaluate(context);

        if (targetRelationContext.Relation is not IWritableRelation writableRelation)
            throw new InvalidOperationException("Cannot insert to non-writable relation");

        var valueRelationContext = ValueRelationNode.Value.Evaluate(context);

        var records = valueRelationContext.Records.ToList();
        writableRelation.AddRecords(records);

        return new AppendRecordActionResult(records.Count);
    }
    
    private record AppendRecordActionResult(int Count): IAppendRecordActionResult
    {
    }
}