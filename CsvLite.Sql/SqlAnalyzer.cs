using Antlr4.Runtime;
using CsvLite.Sql.Actions;

namespace CsvLite.Sql;

public class SqlAnalyzer
{
    public ISqlAction Analyze(string sql)
    {
        var antlrInputStream = new AntlrInputStream(sql);

        return AnalyzeInternal(antlrInputStream);
    }
    
    public ISqlAction Analyze(Stream stream)
    {
        var antlrInputStream = new AntlrInputStream(stream);
        
        return AnalyzeInternal(antlrInputStream);
    }

    private ISqlAction AnalyzeInternal(AntlrInputStream stream)
    {
        var lexer = new SqlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new SqlParser(tokens);

        var root = parser.root();

        return new SelectAction
        {
        };
    }
}
