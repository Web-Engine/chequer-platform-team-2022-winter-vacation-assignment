namespace CsvLite.Models.Tuples;

public interface ITuple
{
    IEnumerable<object> Values { get; }
}
