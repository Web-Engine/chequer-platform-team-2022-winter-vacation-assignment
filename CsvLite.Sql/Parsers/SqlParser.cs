using Antlr4.Runtime;
using CsvLite.Sql.Parsers.Antlr;
using CsvLite.Sql.Parsers.Visitors;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Actions;

namespace CsvLite.Sql.Parsers;

public class SqlParser
{
    public IActionNode Parse(string sql)
    {
        var antlrInputStream = new AntlrInputStream(sql);

        return ParseInternal(antlrInputStream);
    }
    
    public IActionNode Parse(Stream stream)
    {
        var antlrInputStream = new AntlrInputStream(stream);
        
        return ParseInternal(antlrInputStream);
    }

    private IActionNode ParseInternal(AntlrInputStream stream)
    {
        var lexer = new AntlrSqlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new AntlrSqlParser(tokens);
        
        var root = parser.root();

        return ActionVisitor.VisitAction(root);
    }
}