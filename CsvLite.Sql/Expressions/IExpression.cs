using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;

namespace CsvLite.Sql.Expressions;

public interface IExpression
{
    object? Evaluate(IAttribute[] attributes, ITuple tuple);
}
