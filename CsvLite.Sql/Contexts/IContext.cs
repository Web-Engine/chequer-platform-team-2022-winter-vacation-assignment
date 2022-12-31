using CsvLite.Models.Identifiers;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Contexts;

public interface IContext
{
    IContext? Parent { get; }
    
    IRelationContext GetPhysicalContext(Identifier identifier);
}