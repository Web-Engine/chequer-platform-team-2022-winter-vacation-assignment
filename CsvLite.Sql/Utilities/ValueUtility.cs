namespace CsvLite.Sql.Utilities;

public static class ValueUtility
{
    public static bool ToBoolean(object? any)
    {
        if (any == null) return false;

        if (any is IConvertible convertible)
            return convertible.ToBoolean(null);

        return true;
    }

    public static long ToLong(object? any)
    {
        if (any == null) return 0;
        
        if (any is IConvertible convertible)
            return convertible.ToInt64(null);

        throw new InvalidOperationException("Cannot convert to long");
    }

    public static double ToDouble(object? any)
    {
        if (any == null) return 0;
        
        if (any is IConvertible convertible)
            return convertible.ToDouble(null);

        throw new InvalidOperationException("Cannot convert to double");
    }
}