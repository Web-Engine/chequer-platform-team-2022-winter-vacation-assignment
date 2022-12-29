using System.Collections.Immutable;
using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Models.Relations;

public class EmptyRowRelation : IRelation
{
    public IReadOnlyList<IAttribute> Attributes => ImmutableList<IAttribute>.Empty; 

    public IEnumerable<IRecord> Records { get; } = new List<IRecord>
    {
        new DefaultRecord()
    };
}
