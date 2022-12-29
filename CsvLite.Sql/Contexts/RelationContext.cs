using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Models.Relations;

namespace CsvLite.Sql.Contexts;

public sealed class RelationContext : IRelationContext
{
    public IRootContext Parent { get; }
    
    public IRelation Relation { get; }

    internal RelationContext(IRootContext parent, IRelation relation)
    {
        Parent = parent;
        Relation = relation;
    }

    public IPhysicalRelation GetPhysicalRelation(Identifier identifier)
    {
        return Parent.GetPhysicalRelation(identifier);
    }

    public IRelationContext CreateRelationContext(IRelation relation)
    {
        return new RelationContext(this, relation);
    }

    public IRecordContext CreateRecordContext(IRecord record)
    {
        return new RecordContext(this, record);
    }
}