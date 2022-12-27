using CsvLite.Models.Relations;

namespace CsvLite.IO.Relations;

public interface IRelationReader
{
    IPhysicalRelation Read();
}