using CsvLite.Sql;
using CsvLite.Sql.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var analyzer = new SqlAnalyzer();

        // var result = analyzer.Analyze("SELECT * FROM foo WHERE 1 = 1 AND 2 = 1 OR 3 = 1 AND 4 = 1 AND 2 = 1 OR 1 = 1");
        var result = analyzer.Analyze("call(1 AND 2) AND 1");

        if (result is SqlDummyResult sqlDummy)
        {
            Console.WriteLine(sqlDummy.Dummy);
        }
    }
}
