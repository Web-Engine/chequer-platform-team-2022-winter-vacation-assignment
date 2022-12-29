using CsvLite.Models.Records;
using CsvLite.Models.Values.Primitives;
using CsvLite.Utilities;

namespace CsvLite.Models.Values;

public sealed class TupleValue : List<IValue>, IValue
{
    public TupleValue()
    {
    }

    public TupleValue(IEnumerable<IValue> values) : base(values)
    {
    }

    public PrimitiveValue AsPrimitive()
    {
        return Count switch
        {
            0 => NullValue.Null,
            1 => this[0].AsPrimitive(),

            _ => throw new InvalidOperationException($"Cannot convert {GetType()} to PrimitiveValue")
        };
    }

    public TupleValue AsTuple()
    {
        return this;
    }

    public static TupleValue FromRecord(IRecord record)
    {
        return new TupleValue(record);
    }

    public IRecord ToRecord()
    {
        return new DefaultRecord(this);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TupleValue tupleValue)
            return false;

        return this.SequenceEqual(tupleValue);
    }

    public override int GetHashCode()
    {
        return this.Aggregate(0, (sum, value) => sum * 31 + value.GetHashCode());
    }

    public BooleanValue AsBoolean()
    {
        return new BooleanValue(Count != 0);
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