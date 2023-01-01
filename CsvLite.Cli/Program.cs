using System.Collections.Immutable;
using System.Globalization;
using CsvHelper;
using CsvLite.IO.Csv;
using CsvLite.Sql;
using CsvLite.Sql.Engine;
using CsvLite.Sql.Models.Results;
using CsvLite.Sql.Models.Results.Executes;
using CsvLite.Sql.Optimizers;
using CsvLite.Sql.Parsers;

namespace CsvLite.Cli;

public class Program
{
    public static void Main(string[] args)
    {
        var relationProvider = new CsvRelationProvider();
        var executor = new SqlExecutor(relationProvider);

        var relationTextWriter = new RelationTextWriter(Console.Out);

        while (true)
        {
            Console.Write("CQL> ");

            var sql = Console.ReadLine()?.Trim();
            if (sql is null || $"{sql};".StartsWith("exit;", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Goodbye");
                break;
            }

            try
            {
                var result = executor.Execute(sql);

                if (result is IRelationResult relationResult)
                    relationTextWriter.Write(relationResult.Relation);
                else if (result is IAppendRecordsResult appendRecordResult)
                    Console.WriteLine($"{appendRecordResult.Count} Rows Inserted");

                Console.WriteLine($"Time elapsed: {result.Elapsed}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}