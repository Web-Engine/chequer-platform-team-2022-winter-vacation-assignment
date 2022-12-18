using CsvLite.Models.Relations;

namespace CsvLite.IO.Relations;

public interface IRelationReader
{
    IRelation Read();

    Task<IRelation> ReadAsync();
}