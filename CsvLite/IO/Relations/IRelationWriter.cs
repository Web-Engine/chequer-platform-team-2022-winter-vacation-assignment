using CsvLite.Models.Relations;

namespace CsvLite.IO.Relations;

public interface IRelationWriter
{
    void Write(IRelation relation);

    Task WriteAsync(IRelation relation);
}
