namespace CsvLite.Models.Values.Primitives;

public sealed record BooleanValue(bool Value) : PrimitiveValue
{
    public static readonly BooleanValue True = new(true);
    public static readonly BooleanValue False = new(false);
    
    private static readonly StringValue StringTrue = new("TRUE");
    private static readonly StringValue StringFalse = new("False");
    
    public override BooleanValue AsBoolean()
    {
        return this;
    }

    public override StringValue AsString()
    {
        return Value ? StringTrue : StringFalse;
    }

    public override IntegerValue AsInteger()
    {
        return Value ? IntegerValue.Integer1 : IntegerValue.Integer0;
    }
}