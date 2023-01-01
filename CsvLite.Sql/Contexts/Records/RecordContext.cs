using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Contexts.Records;

public sealed class RecordContext : IRecordContext
{
    public IRelationContext Parent { get; }

    public IRelation Relation => Parent.Relation;

    public IRecord Record { get; }

    public IEnumerable<QualifiedIdentifier> AttributeIdentifiers => Parent.AttributeIdentifiers;

    public RecordContext(IRelationContext parent, IRecord record)
    {
        Parent = parent;
        Record = record;
    }

    public IRelationContext GetPhysicalContext(Identifier identifier)
    {
        return Parent.GetPhysicalContext(identifier);
    }
}