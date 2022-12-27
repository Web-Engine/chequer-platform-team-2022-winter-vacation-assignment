namespace CsvLite.Utilities;

public static class EnumerableExtension
{
    public static IEnumerable<(T Value, int Index)> WithIndex<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Select((value, index) => (value, index));
    }
}