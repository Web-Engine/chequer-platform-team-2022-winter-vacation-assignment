using System.Collections.Immutable;
using System.Globalization;
using CsvHelper;
using CsvLite.IO.Csv;
using CsvLite.Sql.Engine;
using CsvLite.Sql.Models.Results;
using CsvLite.Sql.Models.Results.Executes;
using CsvLite.Sql.Optimizers;
using CsvLite.Sql.Parsers;

namespace CsvLite.Cli;

public class Program
{
    // public static void Main(string[] args)
    // {
    //     using var streamReader = new StreamReader("datasets/freshman_kgs.csv");
    //     using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
    //
    //     csvReader.Read();
    //     csvReader.ReadHeader();
    //
    //     var headerRecords = csvReader.HeaderRecord ?? ArraySegment<string>.Empty;
    //     Console.WriteLine(string.Join(",", headerRecords));
    //
    //     while (csvReader.Read())
    //     {
    //         for (var i = 0; i < headerRecords.Count; i++)
    //         {
    //             Console.Write(csvReader.GetField(i));
    //             Console.Write(",\t");
    //         }
    //
    //         Console.WriteLine();
    //     }
    //
    //     Console.WriteLine();
    // }

//     public static void Main(string[] args)
//     {
//         var relationPresenter = new RelationPresenter(Console.Out);
//         var parser = new SqlParser();
//
// //         const string sql = @"
// // SELECT COUNT(*), Sex FROM ""datasets/freshman_lbs.csv"" as kgs GROUP BY ""Sex""
// // ";
//
//         const string sql = @"
//             SELECT
//                 *
//             FROM
//                 ""datasets/freshman_kgs.csv"" as kgs,
//                 ""datasets/freshman_lbs.csv"" as lbs
//             WHERE
//                 (
//                     SELECT
//                          COUNT(*)
//                      FROM
//                         ""datasets/freshman_kgs.csv"" d1,
//                         ""datasets/freshman_kgs.csv"" d2,
//                         ""datasets/freshman_kgs.csv"" d3
//                 ) OR TRUE
//             LIMIT 10
// ";
//
//         var provider = new CsvRelationProvider();
//         var engine = new SqlEngine(provider);
//
//         var optimizer = new RuleBasedOptimizer();
//
//         var action = parser.Parse(sql);
//         action = optimizer.Optimize(action);
//         
//         var result = engine.Execute(action);
//
//         if (result is IRelationResult {Relation: var relation})
//         {
//             relationPresenter.Show(relation);
//         }
//
//         if (result is IAppendRecordResult { Count: var count})
//         {
//             Console.WriteLine($"{count} Records Inserted");
//         }
//
//         Console.WriteLine($"Time elapsed: {result.Elapsed}");
//
// }

    public static void Main(string[] args)
    {
        var relationProvider = new CsvRelationProvider();
        var sqlEngine = new SqlEngine(relationProvider);
        var parser = new SqlParser();
        var optimizer = new RuleBasedOptimizer();
        var relationPresenter = new RelationPresenter(Console.Out);

        while (true)
        {
            Console.Write("CQL> ");

            var sql = Console.ReadLine();
            if (sql is null || sql.Trim().Replace(";", "").Equals("exit"))
            {
                Console.WriteLine("Goodbye");
                break;
            }

            try
            {
                var action = parser.Parse(sql);
                action = optimizer.Optimize(action);

                var result = sqlEngine.Execute(action);

                if (result is IRelationResult relationResult)
                    relationPresenter.Show(relationResult.Relation);

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