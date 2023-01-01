using Antlr4.Runtime;
using CsvLite.Sql.Parsers.Antlr;
using CsvLite.Sql.Test.TestModels;
using NUnit.Framework;

namespace CsvLite.Sql.Test;

public partial class SqlParserTest
{
    private SqlExecutor _executor;
    [SetUp]
    public void Setup()
    {
        _executor = new SqlExecutor(new DummyRelationProvider());
    }

    public void Test_Select()
    {
        
    }
}