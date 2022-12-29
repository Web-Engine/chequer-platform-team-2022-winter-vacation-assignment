using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Contexts;

public sealed class RecordContext : IRecordContext
{
    public IRelationContext Parent { get; }

    public IRelation Relation => Parent.Relation;

    public IRecord Record { get; }

    public RecordContext(IRelationContext parent, IRecord record)
    {
        Parent = parent;
        Record = record;
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
        return Parent.CreateRecordContext(record);
    }
}