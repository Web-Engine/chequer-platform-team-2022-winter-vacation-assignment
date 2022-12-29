using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts;

public class RootContext : IRootContext
{
    public IContext? Parent => null;
    
    private readonly IPhysicalRelationProvider _provider;

    public RootContext(IPhysicalRelationProvider provider)
    {
        _provider = provider;
    }
    
    public IPhysicalRelation GetPhysicalRelation(Identifier identifier)
    {
        return _provider.GetRelation(identifier);
    }

    public IRelationContext CreateRelationContext(IRelation relation)
    {
        return new RelationContext(this, relation);
    }
}