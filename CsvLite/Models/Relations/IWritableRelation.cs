using CsvLite.Models.Records;

namespace CsvLite.Models.Relations;

public interface IWritableRelation : IRelation
{
    public void AddRecord(IRecord record);
}
