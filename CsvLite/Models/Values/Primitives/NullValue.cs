namespace CsvLite.Models.Values.Primitives;

public sealed record NullValue : PrimitiveValue
{
    public static NullValue Null { get; } = new();
    
    public override BooleanValue AsBoolean()
    {
        return BooleanValue.False;
    }

    public override StringValue AsString()
    {
        return StringValue.Empty;
    }

    public override IntegerValue AsInteger()
    {
        return IntegerValue.Integer0;
    }
};
