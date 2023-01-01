using CsvLite.Models.Relations;
using CsvLite.Sql.Engine;
using CsvLite.Sql.Models.Results.Executes;
using CsvLite.Sql.Optimizers;
using CsvLite.Sql.Parsers;

namespace CsvLite.Sql;

public class SqlExecutor
{
    private readonly ISqlParser _parser;
    private readonly IEngine _engine;
    private readonly IOptimizer? _optimizer;

    public SqlExecutor(IRelationProvider provider) : this(
        parser: new SqlParser(),
        engine: new SqlEngine(provider),
        optimizer: new RuleBasedOptimizer()
    )
    {
    }

    public SqlExecutor(
        ISqlParser parser,
        IEngine engine,
        IOptimizer? optimizer = null
    )
    {
        _parser = parser;
        _engine = engine;
        _optimizer = optimizer;
    }

    public IExecuteResult Execute(string sql)
    {
        var node = _parser.Parse(sql);
        var optimized = _optimizer?.Optimize(node) ?? node;

        return _engine.Execute(optimized);
    }
}