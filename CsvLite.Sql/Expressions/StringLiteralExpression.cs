using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Expressions;

public class StringLiteralExpression : IExpression
{
    private readonly string _literal;

    public StringLiteralExpression(string literal)
    {
        _literal = literal;
    }
    
    public object? Evaluate(IAttribute[] attributes, ITuple tuple)
    {
        return _literal;
    }
}