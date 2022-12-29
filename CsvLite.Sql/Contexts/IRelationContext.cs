using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Contexts;

public interface IRelationContext : IRootContext
{
    IContext? IContext.Parent => Parent;

    new IRootContext? Parent { get; }

    IRelation Relation { get; }

    IAttributeList Attributes => Relation.Attributes;

    IEnumerable<IRecord> Records => Relation.Records;

    IRecordContext CreateRecordContext(IRecord record);
}