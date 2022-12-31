using CsvLite.Models.Records;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Contexts.Records;

public interface IRecordContext : IRelationContext
{
    IRecord Record { get; }
}