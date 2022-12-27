namespace CsvLite.Sql.Utilities;

public static class ArrayUtility
{
    public static bool Equals<T>(T[] array1, T[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        return !array1.Except(array2).Any();
    }
}
