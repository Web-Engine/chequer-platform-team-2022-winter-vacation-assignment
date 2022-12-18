using System.Diagnostics.CodeAnalysis;

namespace CsvLite.Models.Domains;

public interface IDomain
{
    bool IsAvailable(object value);
    
    bool TryConvert(string value, [NotNullWhen(true)] out object? converted);
}
