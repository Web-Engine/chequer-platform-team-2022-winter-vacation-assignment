using CsvLite.Models.Schemas;
using CsvLite.Sql.Results;

namespace CsvLite.Sql.Actions;

public interface IRelationAction: ISqlAction
{
    ISqlActionResult ISqlAction.Execute(ISchema schema)
    {
        return Execute(schema);
    }

    new IRelationResult Execute(ISchema schema);
}