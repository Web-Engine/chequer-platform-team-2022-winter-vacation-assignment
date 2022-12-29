using CsvLite.Models.Records;

namespace CsvLite.Sql.Contexts;

public interface IRecordContext : IRelationContext
{
    IRootContext? IRelationContext.Parent => Parent;
    
    new IRelationContext Parent { get; }

    IRecord Record { get; }
}