using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Contexts;

public sealed class RecordContext : IRecordContext
{
    IContext? IContext.Parent => Parent;

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