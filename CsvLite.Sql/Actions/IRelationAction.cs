using CsvLite.Models.Relations;
using CsvLite.Models.Schemas;
using CsvLite.Sql.Models.Results;

namespace CsvLite.Sql.Actions;

public interface IRelationAction: ISqlAction
{
    ISqlActionResult ISqlAction.Execute(IPhysicalRelationProvider schema)
    {
        return Execute(schema);
    }

    new IRelationResult Execute(IPhysicalRelationProvider schema);
}