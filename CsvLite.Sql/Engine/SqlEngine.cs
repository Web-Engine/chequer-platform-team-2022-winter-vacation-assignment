using System.Diagnostics;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Results;
using CsvLite.Sql.Tree.Actions;

namespace CsvLite.Sql.Engine;

public class SqlEngine
{
    private readonly IPhysicalRelationProvider _provider;

    public SqlEngine(IPhysicalRelationProvider provider)
    {
        _provider = provider;
    }

    public IExecuteResult Execute(IActionNode node)
    {
        return node switch
        {
            IRelationActionNode relationAction => ExecuteRelationAction(relationAction),
            IAppendRecordActionNode appendRecordAction => ExecuteAppendRecordAction(appendRecordAction),

            _ => throw new NotSupportedException($"SqlEngine does not support {node.GetType()}")
        };
    }

    private IRelationResult ExecuteRelationAction(IRelationActionNode node)
    {
        var stopwatch = Stopwatch.StartNew();

        var context = new RootContext(_provider);
        var relation = node.RelationNode.Evaluate(context);

        stopwatch.Stop();

        return new RelationResult(relation, stopwatch.Elapsed);
    }

    private IAppendRecordResult ExecuteAppendRecordAction(IAppendRecordActionNode node)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var context = new RootContext(_provider);
        var relation1 = node.TargetRelationNode.Evaluate(context);

        if (relation1 is not IWritableRelation writableRelation)
            throw new InvalidOperationException("Cannot insert to non-writable relation");

        var relation2 = node.ValueRelationNode.Evaluate(context);
        
        var records = relation2.Records.ToList();
        
        writableRelation.AddRecords(records);
        
        stopwatch.Stop();

        return new AppendRecordResult(
            records.Count,
            stopwatch.Elapsed
        );
    }
}
