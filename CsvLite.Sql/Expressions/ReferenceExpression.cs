using CsvLite.Models.Attributes;
using CsvLite.Models.Tuples;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.Expressions;

public class ReferenceExpression : IExpression
{
    private readonly string[] _identifiers;

    public ReferenceExpression(string[] identifiers)
    {
        _identifiers = identifiers;
    }
    
    public object? Evaluate(IAttribute[] attributes, ITuple tuple)
    {
        for (var i = 0; i < attributes.Length; i++)
        {
            if (!ArrayUtility.Equals(attributes[i].Alias, _identifiers)) continue;

            return tuple.Values.ElementAt(i);
        }

        throw new InvalidOperationException($"Cannot find {string.Join(".", _identifiers)}");
    }
}
