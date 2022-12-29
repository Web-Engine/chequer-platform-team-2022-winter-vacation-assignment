using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;

namespace CsvLite.Sql.Contexts;

public interface IRelationContext : IRootContext
{
    IRelation Relation { get; }

    IReadOnlyList<IAttribute> Attributes => Relation.Attributes;

    IEnumerable<IRecord> Records => Relation.Records;

    IEnumerable<QualifiedIdentifier> AttributeIdentifiers { get; }
}