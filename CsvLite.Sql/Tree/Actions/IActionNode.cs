using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Results.Actions;

namespace CsvLite.Sql.Tree.Actions;

public interface IActionNode : INode
{
    IActionResult Execute(IContext context);
}