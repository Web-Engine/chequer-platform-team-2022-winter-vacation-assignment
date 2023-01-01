using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvLite.Sql.Test.TestModels;
using NUnit.Framework;

namespace CsvLite.Sql.Test;

public partial class SqlExecutorTest
{
    private static InsertTestData[]? _testDataSourceInsert;

    public static InsertTestData[] TestDataSourceInsert
    {
        get
        {
            _testDataSourceInsert ??= CreateTestDataSourceInsert();
            return _testDataSourceInsert;
        }
    }

    private static InsertTestData[] CreateTestDataSourceInsert()
    {
        const string directory = "Resources/TestCases/Insert";
        var files = Directory.EnumerateFiles(directory);

        return files
            .Where(file => Path.GetExtension(file).Equals(".sql", StringComparison.OrdinalIgnoreCase))
            .Select(Path.GetFileNameWithoutExtension)
            .OrderBy(filename => filename)
            .Select(filename =>
            {
                var sqlFile = Path.Join(directory, $"{filename}.sql");
                var resultFile = Path.Join(directory, $"{filename}.csv");

                return (Sql: sqlFile, Result: resultFile);
            })
            .Select(tuple =>
            {
                var sql = File.ReadAllText(tuple.Sql);
                var result = File.Exists(tuple.Result) ? ReadLineTrimmedText(tuple.Result) : string.Empty;

                return new InsertTestData(sql, result, "platform_team.csv");
            })
            .ToArray();
    }

    private static string ReadLineTrimmedText(string filePath)
    {
        var lines = File.ReadLines(filePath);

        return string.Join(
            '\n',
            lines
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
        );
    }

    public class InsertTestData
    {
        public string Sql { get; }

        public string RelationName { get; }

        public string Expected { get; }

        public InsertTestData(string sql, string expect, string relationName)
        {
            Sql = sql.Trim();
            RelationName = relationName;

            Expected = expect;
        }

        public void AssertCsv(ResourceRelationProvider provider)
        {
            var filePath = provider.GetRelationFilePath(RelationName);

            var result = ReadLineTrimmedText(filePath);

            if (Expected.Equals(result))
            {
                Assert.Pass();
                return;
            }

            Console.WriteLine($"Returned:");
            Console.WriteLine(result);
            Console.WriteLine();

            Console.WriteLine($"Expected:");
            Console.WriteLine(Expected);
            Console.WriteLine();

            Assert.Fail("Wrong Csv Returned");
        }
    }
}