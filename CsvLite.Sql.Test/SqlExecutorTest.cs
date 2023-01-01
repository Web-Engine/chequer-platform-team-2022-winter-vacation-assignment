using System;
using CsvLite.Sql.Models.Results.Executes;
using CsvLite.Sql.Test.TestModels;
using NUnit.Framework;

namespace CsvLite.Sql.Test;

public partial class SqlExecutorTest
{
    private ResourceRelationProvider _provider = null!;
    private SqlExecutor _executor = null!;

    [SetUp]
    public void Setup()
    {
        _provider = new ResourceRelationProvider();
        _executor = new SqlExecutor(_provider);
    }

    [TearDown]
    public void TearDown()
    {
        _provider.Dispose();
    }

    [TestCaseSource(nameof(TestDataSourceSourceSelect))]
    public void Test_Select(SelectTestData test)
    {
        Console.WriteLine($"SQL: {test.Sql}");
        
        var result = _executor.Execute(test.Sql);

        if (result is not IRelationResult relationResult)
        {
            Assert.Fail($"Wrong result type {result.GetType()}");
            return;
        }

        test.AssertRelation(relationResult.Relation);
    }

    [TestCaseSource(nameof(TestDataSourceInsert))]
    public void Test_Insert(InsertTestData test)
    {
        Console.WriteLine($"SQL: {test.Sql}");
        
        var result = _executor.Execute(test.Sql);

        if (result is not IAppendRecordsResult recordsResult)
        {
            Assert.Fail($"Wrong result type {result.GetType()}");
            return;
        }

        test.AssertCsv(_provider);
    }
}