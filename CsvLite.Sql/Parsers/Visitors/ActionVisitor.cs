using CsvLite.Sql.Actions;
using static CsvLite.Sql.Parsers.Antlr.AntlrSqlParser;

namespace CsvLite.Sql.Parsers.Visitors;

public static class ActionVisitor
{
    public static ISqlAction VisitAction(RootContext context)
    {
        var statement = context.statement();

        return statement switch
        {
            Statement_selectContext selectContext => VisitStatement_select(selectContext),
            
            _ => throw new InvalidOperationException("Unknown statement")
        };
    }

    private static ISqlAction VisitStatement_select(Statement_selectContext context)
    {       
        var relationNode = RelationVisitor.VisitStatementSelect(context.statementSelect());
        
        return new SelectAction(
            relationNode
        );
    }
}