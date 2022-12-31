using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public class CombineRelationContext : IRelationContext
{
    public IContext? Parent => Context1.Parent;

    public IRelation Relation { get; }

    public IRelationContext Context1 { get; }
    
    public IRelationContext Context2 { get; }

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers
    {
        get
        {
            foreach (var identifier in Context1.AttributeIdentifiers)
            {
                yield return identifier;
            }

            foreach (var identifier in Context2.AttributeIdentifiers)
            {
                yield return identifier;
            }
        }
    }

    public CombineRelationContext(IRelationContext context1, IRelationContext context2, IRelation relation)
    {
        Context1 = context1;
        Context2 = context2;

        Relation = relation;
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Context1.GetPhysicalContext(identifier);
    }
}