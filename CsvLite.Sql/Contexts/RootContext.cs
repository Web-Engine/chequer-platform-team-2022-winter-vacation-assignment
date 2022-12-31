using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Contexts;

public class RootContext : IContext
{
    public IContext? Parent => null;
    
    private readonly IPhysicalRelationProvider _provider;

    public RootContext(IPhysicalRelationProvider provider)
    {
        _provider = provider;
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return new NamedRelationContext(this, identifier, _provider.GetRelation(identifier));
    }
}