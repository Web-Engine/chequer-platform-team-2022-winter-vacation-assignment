namespace CsvLite.Models.Values.Primitives;

public sealed record IntegerValue(int Value) : PrimitiveValue
{
    public static readonly IntegerValue Integer0 = new(0);
    public static readonly IntegerValue Integer1 = new(1);

    public override BooleanValue AsBoolean()
    {
        return Value != 0 ? BooleanValue.True : BooleanValue.False;
    }

    public override StringValue AsString()
    {
        return new StringValue(Value.ToString());
    }

    public override IntegerValue AsInteger()
    {
        return this;
    }
}