using CsvLite.Models.Records;

namespace CsvLite.Sql.Contexts;

public interface IRecordContext : IRelationContext
{
    IRecord Record { get; }
}