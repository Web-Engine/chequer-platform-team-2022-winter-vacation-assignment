using CsvLite.Models.Schemas;

namespace CsvLite.Sql.Projections;

public interface IProjection
{
    string Schema { get; }
    
    string Relation { get; }
    
    string Attribute { get; }
    
    string Alias { get; }
}
