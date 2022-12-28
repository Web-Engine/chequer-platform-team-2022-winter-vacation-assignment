using CsvLite.Models.Relations;
using CsvLite.Models.Schemas;
using CsvLite.Sql.Models.Results;

namespace CsvLite.Sql.Actions;

public interface IRelationAction: ISqlAction
{
    ISqlActionResult ISqlAction.Execute(IRelationProvider schema)
    {
        return Execute(schema);
    }

    new IRelationResult Execute(IRelationProvider schema);
}