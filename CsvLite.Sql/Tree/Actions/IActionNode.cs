using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Results;
using CsvLite.Sql.Models.Results.Actions;
using CsvLite.Sql.Models.Results.Executes;

namespace CsvLite.Sql.Tree.Actions;

public interface IActionNode : INode
{
    IActionResult Execute(IRootContext context);
}