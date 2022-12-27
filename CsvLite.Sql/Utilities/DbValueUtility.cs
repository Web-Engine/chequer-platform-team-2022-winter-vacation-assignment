using CsvLite.Models.Values;

namespace CsvLite.Sql.Utilities;

public static class DbValueUtility
{
    public static BooleanValue AsBoolean(this IValue value)
    {
        if (value is BooleanValue dbBoolean)
            return dbBoolean;

        if (value is NullValue)
            return new BooleanValue(false);

        if (value is IntegerValue dbInteger)
            return new BooleanValue(dbInteger.Value != 0);

        if (value is StringValue dbString)
        {
            var trim = dbString.Value.Trim();
            
            if ("TRUE".Equals(trim, StringComparison.OrdinalIgnoreCase))
                return new BooleanValue(true);

            if ("FALSE".Equals(trim, StringComparison.OrdinalIgnoreCase))
                return new BooleanValue(false);
            
            if ("1".Equals(trim, StringComparison.OrdinalIgnoreCase))
                return new BooleanValue(true);

            if ("0".Equals(trim, StringComparison.OrdinalIgnoreCase))
                return new BooleanValue(false);
            
            return new BooleanValue(dbString.Value.Length != 0);
        }

        throw new InvalidOperationException("Wrong DbValue Type");
    }

    public static IntegerValue AsInteger(this IValue value)
    {
        if (value is IntegerValue dbInteger)
            return dbInteger;
        
        if (value is BooleanValue dbBoolean)
            return new IntegerValue(dbBoolean.Value ? 1 : 0);

        if (value is NullValue)
            return new IntegerValue(0);

        if (value is StringValue dbString)
        {
            if (!int.TryParse(dbString.Value, out var parse))
                throw new InvalidOperationException("Cannot convert DbString to DbInteger");

            return new IntegerValue(parse);
        }

        throw new InvalidOperationException("Wrong DbValue Type");
    }

    public static StringValue AsString(this IValue value)
    {
        if (value is StringValue dbString)
            return dbString;

        if (value is NullValue)
            return new StringValue("");

        if (value is BooleanValue dbBoolean)
            return new StringValue(dbBoolean.Value ? "TRUE" : "FALSE");

        if (value is IntegerValue dbInteger)
            return new StringValue(dbInteger.ToString());
        
        throw new InvalidOperationException("Wrong DbValue Type");
    }
}