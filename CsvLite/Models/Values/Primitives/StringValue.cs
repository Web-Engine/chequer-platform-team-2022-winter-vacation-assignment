namespace CsvLite.Models.Values.Primitives;

public sealed record StringValue(string Value) : PrimitiveValue
{
    public static readonly StringValue Empty = new("");

    public override BooleanValue AsBoolean()
    {
        return Value.Length == 0 ? BooleanValue.False : BooleanValue.True;
    }

    public override StringValue AsString()
    {
        return this;
    }

    public override IntegerValue AsInteger()
    {
        if (!int.TryParse(Value, out var integer))
            throw new InvalidOperationException($"Cannot convert {Value} as integer");

        return new IntegerValue(integer);
    }
}