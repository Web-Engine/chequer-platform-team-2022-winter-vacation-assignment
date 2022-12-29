using CsvLite.Sql.Models.Results.Executes;
using CsvLite.Sql.Tree.Actions;

namespace CsvLite.Sql.Engine;

public interface IEngine
{
    IExecuteResult Execute(IActionNode node);
}