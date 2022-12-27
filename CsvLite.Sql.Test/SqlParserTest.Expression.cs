using Antlr4.Runtime;
using CsvLite.Sql.Parsers.Antlr;
using NUnit.Framework;

namespace CsvLite.Sql.Test;

public partial class SqlParserTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(@"1", @"(expression (expressionValue (expressionPrimitive (integer 1))))")]
    [TestCase(@"2", @"(expression (expressionValue (expressionPrimitive (integer 2))))")]
    [TestCase(@"'a'", @"(expression (expressionValue (expressionPrimitive (text 'a'))))")]
    [TestCase(@"'abc'", @"(expression (expressionValue (expressionPrimitive (text 'abc'))))")]
    [TestCase(@"'a\'b'", @"(expression (expressionValue (expressionPrimitive (text 'a\'b'))))")]
    [TestCase(@"1.1", @"(expression (expressionValue (expressionPrimitive (double 1.1))))")]
    public void Test_Expression(string expression, string value)
    {
        var stream = new AntlrInputStream(expression);
        var lexer = new AntlrSqlLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new AntlrSqlParser(tokens);

        var context = parser.expression();
        
        Assert.AreEqual(context.ToStringTree(parser), value);
    }
}