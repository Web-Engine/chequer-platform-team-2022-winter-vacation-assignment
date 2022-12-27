using Antlr4.Runtime;
using CsvLite.Sql.Actions;
using CsvLite.Sql.Parsers.Antlr;
using CsvLite.Sql.Parsers.Visitors;

namespace CsvLite.Sql.Parsers;

public class SqlParser
{
    public ISqlAction Parse(string sql)
    {
        var antlrInputStream = new AntlrInputStream(sql);

        return ParseInternal(antlrInputStream);
    }
    
    public ISqlAction Parse(Stream stream)
    {
        var antlrInputStream = new AntlrInputStream(stream);
        
        return ParseInternal(antlrInputStream);
    }

    private ISqlAction ParseInternal(AntlrInputStream stream)
    {
        var lexer = new AntlrSqlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new AntlrSqlParser(tokens);
        
        var root = parser.root();

        return ActionVisitor.VisitAction(root);
    }
}