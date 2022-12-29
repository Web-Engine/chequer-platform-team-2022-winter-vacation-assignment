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

    public static PrimitiveValue Parse(string? value)
    {
        if (value is null)
            return NullValue.Null;
        
        switch (value.ToUpper())
        {
            case "TRUE":
                return BooleanValue.True;
            case "FALSE":
                return BooleanValue.False;
        }

        if (int.TryParse(value, out var integer))
            return new IntegerValue(integer);

        return new StringValue(value);
    }
}