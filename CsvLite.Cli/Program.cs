using CsvLite.IO.Csv;
using CsvLite.Sql.Models.Results;
using CsvLite.Sql.Parsers;

namespace CsvLite.Cli;

public class Program
{
    public static void Main(string[] args)
    {
        var relationPresenter = new RelationPresenter(Console.Out);
        var parser = new SqlParser();
        
        
        const string sql = @"
            SELECT
                *
            FROM
                ""datasets/freshman_kgs.csv"" as kgs,
                (SELECT * FROM ""datasets/freshman_lbs.csv"" WHERE Sex = 'F' LIMIT 10) as lbs
            WHERE
                kgs.Sex = 'M'
            LIMIT 100
";

        var action = parser.Parse(sql);
        var result = action.Execute(new CsvRelationProvider());

        if (result is not IRelationResult relationResult)
            throw new Exception("Wrong!");

        relationPresenter.Show(relationResult.Relation);

        // relationPresenter.Show(relation);

        // using var writer = new CsvWriter("copied.csv");
        // writer.Write(relation);

        // var provider = new CsvRelationProvider();
        //
        // var parser = new SqlParser();
        // while (true)
        // {
        //     Console.Write("CQL> ");
        //
        //     var sql = Console.ReadLine();
        //     if (sql.Trim().Replace(";", "").Equals("exit"))
        //     {
        //         Console.WriteLine("Goodbye");
        //         break;
        //     }
        //
        //     try
        //     {
        //         var action = parser.Parse(sql);
        //
        //         var result = action.Execute(provider);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //     }
        // }
        //
        // Console.WriteLine("Done");
        // action.Execute(provider);
    }
}