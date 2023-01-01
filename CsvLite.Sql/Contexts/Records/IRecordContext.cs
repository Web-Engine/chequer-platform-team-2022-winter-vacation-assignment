using CsvLite.Models.Records;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Contexts.Records;

public interface IRecordContext : IRelationContext
{
    IContext IRelationContext.Parent => Parent;

    new IRelationContext Parent { get; }

    IRecord Record { get; }
}