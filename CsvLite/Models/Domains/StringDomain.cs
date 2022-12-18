namespace CsvLite.Models.Domains;

public class StringDomain : IDomain
{
    public bool IsAvailable(object value)
    {
        return value is string;
    }

    public bool TryConvert(string value, out object converted)
    {
        converted = value;
        return true;
    }
}
