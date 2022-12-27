using CsvLite.Models.Identifiers;
using CsvLite.Utilities;
using static CsvLite.Sql.Parsers.Antlr.AntlrSqlParser;

namespace CsvLite.Sql.Parsers.Visitors;

public static class ContextExtension
{
    public static Identifier ToIdentifier(this IdentifierContext context)
    {
        switch (context)
        {
            case Identifier_unquotedContext unquoted:
                return new Identifier(
                    unquoted.IDENTIFIER().GetText()
                );

            case Identifier_quotedContext quoted:
                return new Identifier(
                    Identifier.Unescape(quoted.DOUBLE_QUOTED_TEXT().GetText())
                );
        }

        throw new InvalidOperationException("Unknown identifierContext");
    }

    public static string GetString(this LiteralStringContext context)
    {
        return StringUtility.Unescape(context.GetText(), "\\\'", '\'');
    }

    public static int GetInteger(this LiteralIntegerContext context)
    {
        return int.Parse(context.GetText());
    }

    public static bool GetBoolean(this LiteralBooleanContext context)
    {
        return context.TRUE() is { };
    }
}