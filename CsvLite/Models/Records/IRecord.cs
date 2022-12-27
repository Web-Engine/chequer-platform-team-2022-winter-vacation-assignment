using CsvLite.Models.Attributes;
using CsvLite.Models.Values;

namespace CsvLite.Models.Records;

public interface IRecord : IReadOnlyList<IValue>
{
}
