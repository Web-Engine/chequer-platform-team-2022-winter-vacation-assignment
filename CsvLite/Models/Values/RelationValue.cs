using CsvLite.Models.Relations;

namespace CsvLite.Models.Values;

public sealed class RelationValue : IValue
{
    public IRelation Relation { get; }

    public RelationValue(IRelation relation)
    {
        Relation = relation;
    }

    public PrimitiveValue AsPrimitive()
    {
        return AsTuple().AsPrimitive();
    }

    public TupleValue AsTuple()
    {
        if (Relation.Attributes.Count == 0)
            return new TupleValue();

        var records = Relation.Records.Take(2).ToList();

        if (records.Count == 0)
            return new TupleValue();

        if (records.Count != 1)
            throw new InvalidOperationException($"Cannot convert {GetType()} to TupleValue");

        return new TupleValue(records[0]);
    }
}