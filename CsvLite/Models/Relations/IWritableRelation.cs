using CsvLite.Models.Records;

namespace CsvLite.Models.Relations;

public interface IWritableRelation : IRelation
{
    public void AddRecords(IEnumerable<IRecord> records);
}
