using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Models.Records;

namespace CsvLite.Sql.Models.Relations;

public class ComplexRelation : IRelation
{
    public IAttributeList Attributes { get; }

    public IEnumerable<IRecord> Records { get; }

    public ComplexRelation(IRelation relation1, IRelation relation2)
    {
        Attributes = new ConcatAttributeList(relation1.Attributes, relation2.Attributes);

        Records = relation1.Records.SelectMany(
            _ => relation2.Records,
            (record1, record2) => new ConcatRecord(record1, record2)
        );
    }
}
