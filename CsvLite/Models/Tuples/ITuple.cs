namespace CsvLite.Models.Tuples;

public interface ITuple : IReadOnlyTuple
{
    IEnumerable<object> IReadOnlyTuple.Values => Values;
    
    new IList<object> Values { get; set; }
}
