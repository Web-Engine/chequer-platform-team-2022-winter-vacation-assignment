using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Models.Relations;

public class InheritRelation : IRelation
{
    public IAttributeList Attributes { get; }

    public IEnumerable<IRecord> Records { get; }

    public InheritRelation(IRelationContext context, IAttributeList? attributes = null,
        IEnumerable<IRecord>? records = null)
        : this(context.Relation, attributes, records)
    {
    }

    public InheritRelation(IRelation relation, IAttributeList? attributes = null, IEnumerable<IRecord>? records = null)
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
        Attributes = new DefaultAttributeList(
            relation.Attributes
                .Where(attribute => attributeFilter?.Invoke(attribute) ?? true)
                .Select(attribute => attributeTransformer?.Invoke(attribute) ?? attribute)
        );

        Records = relation.Records
            .Where(record => recordFilter?.Invoke(record) ?? true)
            .Select(record => recordTransformer?.Invoke(record) ?? record);
    }
}