using System;
using System.IO;
using System.Linq;
using CsvLite.Models.Relations;
using NUnit.Framework;

namespace CsvLite.Sql.Test;

public partial class SqlExecutorTest
{
    private static SelectTestData[]? _testDataSourceSelect;

    public static SelectTestData[] TestDataSourceSourceSelect
    {
        get
        {
            _testDataSourceSelect ??= CreateTestDataSourceSelect();
            return _testDataSourceSelect;
        }
    }

    private static SelectTestData[] CreateTestDataSourceSelect()
    {
        const string directory = "Resources/TestCases/Select";
        var files = Directory.EnumerateFiles(directory);

        return files
            .Where(file => Path.GetExtension(file).Equals(".sql", StringComparison.OrdinalIgnoreCase))
            .Select(Path.GetFileNameWithoutExtension)
            .OrderBy(filename => filename)
            .Select(filename =>
            {
                var sqlFile = Path.Join(directory, $"{filename}.sql");
                var resultFile = Path.Join(directory, $"{filename}.txt");

                return (Sql: sqlFile, Result: resultFile);
            })
            .Select(tuple =>
            {
                var sql = File.ReadAllText(tuple.Sql);
                var result = File.Exists(tuple.Result) ? File.ReadAllText(tuple.Result) : string.Empty;

                return new SelectTestData(sql, result);
            })
            .ToArray();
    }

    public class SelectTestData
    {
        public string Sql { get; }
        public string Expected { get; }

        public SelectTestData(string sql, string expect)
        {
            Sql = sql.Trim();
            Expected = expect.Trim();
        }

        public void AssertRelation(IRelation relation)
        {
            var writer = new StringWriter();
            var presenter = new RelationTextWriter(writer);

            presenter.Write(relation);

            var result = writer.ToString().Trim();

            if (Expected.Equals(result))
            {
                Assert.Pass();
                return;
            }

            Console.WriteLine("Returned: ");
            Console.WriteLine(result);
            Console.WriteLine();
            
            Console.WriteLine("Expected: ");
            Console.WriteLine(Expected);
            Console.WriteLine();
            
            Assert.Fail("Wrong Relation Returned");
        }
    }
}