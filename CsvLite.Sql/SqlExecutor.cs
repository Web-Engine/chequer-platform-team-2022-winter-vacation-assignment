using CsvLite.Models.Relations;
using CsvLite.Sql.Engine;
using CsvLite.Sql.Models.Results.Executes;
using CsvLite.Sql.Optimizers;
using CsvLite.Sql.Parsers;

namespace CsvLite.Sql;

public class SqlExecutor
{
    private readonly ISqlEngine _engine;
    private readonly ISqlParser _parser;
    private readonly ISqlOptimizer? _optimizer;

    public SqlExecutor(
        IPhysicalRelationProvider provider
    ) : this(
        new SqlEngine(provider),
        new SqlParser(),
        new RuleBasedOptimizer()
    )
    {
    }

    public SqlExecutor(
        ISqlEngine engine,
        ISqlParser parser,
        ISqlOptimizer? optimizer = null
    )
    {
        _engine = engine;
        _parser = parser;
        _optimizer = optimizer;
    }

    public IExecuteResult Execute(string sql)
    {
        var node = _parser.Parse(sql);
        var optimized = _optimizer?.Optimize(node) ?? node;

        return _engine.Execute(optimized);
    }
}