using CsvLite.Sql.Tree.Actions;
using CsvLite.Sql.TreeImpl.Actions;
using static CsvLite.Sql.Parsers.Antlr.AntlrSqlParser;

namespace CsvLite.Sql.Parsers.Visitors;

public static class ActionVisitor
{
    public static IActionNode VisitAction(RootContext context)
    {
        var statement = context.statement();

        return statement switch
        {
            Statement_selectContext select => VisitStatement_select(select),
            Statement_insertContext insert => VisitStatement_insert(insert),
            
            _ => throw new InvalidOperationException("Unknown statement")
        };
    }

    private static IActionNode VisitStatement_select(Statement_selectContext context)
    {       
        var relationNode = RelationVisitor.VisitStatementSelect(context.statementSelect());
        
        return new SelectActionNode(
            relationNode
        );
    }

    private static IActionNode VisitStatement_insert(Statement_insertContext context)
    {
        return VisitStatementInsert(context.statementInsert());
    }

    private static IActionNode VisitStatementInsert(StatementInsertContext context)
    {
        var targetRelation = RelationVisitor.VisitInsertItem(context.insertItem());
        var valueRelation = RelationVisitor.VisitInsertValues(context.insertValues());

        return new AppendRecordActionNode(
            targetRelation,
            valueRelation
        );
    }
}