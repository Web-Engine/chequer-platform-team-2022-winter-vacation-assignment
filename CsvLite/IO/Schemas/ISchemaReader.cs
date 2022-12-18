using CsvLite.Models.Schemas;

namespace CsvLite.IO.Schemas;

public interface ISchemaReader
{
    ISchema Read();
    
    Task<ISchema> ReadAsync();
}