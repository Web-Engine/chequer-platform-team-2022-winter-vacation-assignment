namespace CsvLite.Models.Tuples;

public interface IReadOnlyTuple
{
    IEnumerable<object> Values { get; }
}