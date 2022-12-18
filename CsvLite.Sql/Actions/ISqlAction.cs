using CsvLite.Models.Schemas;
using CsvLite.Sql.Results;

namespace CsvLite.Sql.Actions;

public interface ISqlAction
{
    ISqlActionResult Execute(ISchema scheme);
}