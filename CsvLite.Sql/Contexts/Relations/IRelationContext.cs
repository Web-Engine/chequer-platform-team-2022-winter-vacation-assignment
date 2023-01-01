using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Contexts.Relations;

public interface IRelationContext : IContext
{
    IContext? IContext.Parent => Parent;
    
    new IContext Parent { get; }

    IRelation Relation { get; }

    IReadOnlyList<IAttribute> Attributes => Relation.Attributes;

    IEnumerable<IRecord> Records => Relation.Records;

    IEnumerable<QualifiedIdentifier> AttributeIdentifiers { get; }
}