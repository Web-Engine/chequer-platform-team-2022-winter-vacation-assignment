using CsvLite.Models.Records;

namespace CsvLite.Models.Relations;

public interface IPhysicalRelation : IRelation
{
    public void AddRecord(IRecord record);
}
