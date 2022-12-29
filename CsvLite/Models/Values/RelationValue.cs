using CsvLite.Models.Relations;
using CsvLite.Models.Values.Primitives;

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

    public BooleanValue AsBoolean()
    {
        return new BooleanValue(Relation.Attributes.Count != 0 && Relation.Records.Any());
    }

    public IntegerValue AsInteger()
    {
        return AsPrimitive().AsInteger();
    }

    public StringValue AsString()
    {
        return AsPrimitive().AsString();
    }
}