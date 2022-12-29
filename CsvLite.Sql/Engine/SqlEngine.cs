using System.Diagnostics;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Results.Actions;
using CsvLite.Sql.Models.Results.Executes;
using CsvLite.Sql.Tree.Actions;

namespace CsvLite.Sql.Engine;

public class SqlEngine : IEngine
{
    private readonly IPhysicalRelationProvider _provider;

    public SqlEngine(IPhysicalRelationProvider provider)
    {
        _provider = provider;
    }

    public IExecuteResult Execute(IActionNode node)
    {
        var stopwatch = Stopwatch.StartNew();

        var context = new RootContext(_provider);
        var actionResult = node.Execute(context);

        stopwatch.Stop();

        return actionResult switch
        {
            IRelationActionResult {Relation: var relation} =>
                new RelationResult(relation, stopwatch.Elapsed),

            IAppendRecordActionResult {Count: var count} =>
                new AppendRecordsResult(count, stopwatch.Elapsed),

            _ => throw new InvalidOperationException($"Wrong Action Result {actionResult.GetType()}")
        };
    }

    private class RelationResult : IRelationResult
    {
        public IRelation Relation { get; }
        public TimeSpan Elapsed { get; }

        public RelationResult(IRelation relation, TimeSpan elapsed)
        {
            Relation = relation;
            Elapsed = elapsed;
        }
    }

    private class AppendRecordsResult : IAppendRecordsResult
    {
        public int Count { get; }
        public TimeSpan Elapsed { get; }

        public AppendRecordsResult(int count, TimeSpan elapsed)
        {
            Count = count;
            Elapsed = elapsed;
        }
    }
}