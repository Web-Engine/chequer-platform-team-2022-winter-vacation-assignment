using CsvLite.Models.Identifiers;

namespace CsvLite.Sql.Contexts;

public interface IRootContext : IContext
{
    IRelationContext GetPhysicalContext(Identifier identifier);
}