using CsvLite.Models.Values.Primitives;

namespace CsvLite.Models.Values;

public abstract record PrimitiveValue : IValue
{
    public abstract BooleanValue AsBoolean();

    public abstract StringValue AsString();

    public abstract IntegerValue AsInteger();

    public PrimitiveValue AsPrimitive()
    {
        return this;
    }

    public TupleValue AsTuple()
    {
        return new TupleValue(new[] {this});
    }
}