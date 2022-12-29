using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts;

public interface IRootContext : IContext
{
    IPhysicalRelation GetPhysicalRelation(Identifier identifier);

    IRelationContext CreateRelationContext(IRelation relation);
}