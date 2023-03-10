using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Models.Relations;

public class InheritRelation : IRelation
{
    public IReadOnlyList<IAttribute> Attributes { get; }

    public IEnumerable<IRecord> Records { get; }

    public InheritRelation(
        IRelationContext context,
        IReadOnlyList<IAttribute>? attributes = null,
        IEnumerable<IRecord>? records = null
    )
        : this(context.Relation, attributes, records)
    {
    }

    public InheritRelation(
        IRelation relation,
        IReadOnlyList<IAttribute>? attributes = null,
        IEnumerable<IRecord>? records = null
    )
    {
        Attributes = attributes ?? relation.Attributes;
        Records = records ?? relation.Records;
    }

    public InheritRelation(
        IRelationContext context,
        Func<IAttribute, bool>? attributeFilter = null,
        Func<IAttribute, IAttribute>? attributeTransformer = null,
        Func<IRecord, bool>? recordFilter = null,
        Func<IRecord, IRecord>? recordTransformer = null
    ) : this(context.Relation, attributeFilter, attributeTransformer, recordFilter, recordTransformer)
    {
    }

    public InheritRelation(
        IRelation relation,
        Func<IAttribute, bool>? attributeFilter = null,
        Func<IAttribute, IAttribute>? attributeTransformer = null,
        Func<IRecord, bool>? recordFilter = null,
        Func<IRecord, IRecord>? recordTransformer = null
    )
    {
        Attributes = relation.Attributes
            .Where(attribute => attributeFilter?.Invoke(attribute) ?? true)
            .Select(attribute => attributeTransformer?.Invoke(attribute) ?? attribute)
            .ToList();

        Records = relation.Records
            .Where(record => recordFilter?.Invoke(record) ?? true)
            .Select(record => recordTransformer?.Invoke(record) ?? record);
    }
}