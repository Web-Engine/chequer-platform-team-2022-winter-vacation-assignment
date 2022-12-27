using CsvLite.Models.Relations;
using CsvLite.Models.Schemas;
using CsvLite.Sql.Models.Results;

namespace CsvLite.Sql.Actions;

public interface ISqlAction
{
    ISqlActionResult Execute(IPhysicalRelationProvider scheme);
}