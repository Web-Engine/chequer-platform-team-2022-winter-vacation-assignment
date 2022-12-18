using CsvLite.Models.Schemas;

namespace CsvLite.IO.Schemas;

public interface ISchemaWriter
{
    void Write(ISchema schema);
    
    Task WriteAsync(ISchema schema);
}