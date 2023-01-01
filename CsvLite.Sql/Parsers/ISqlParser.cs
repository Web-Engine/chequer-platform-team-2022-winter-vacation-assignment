using CsvLite.Sql.Tree.Actions;

namespace CsvLite.Sql.Parsers;

public interface ISqlParser
{
    IActionNode Parse(string sql);
    
    IActionNode Parse(Stream stream);
}