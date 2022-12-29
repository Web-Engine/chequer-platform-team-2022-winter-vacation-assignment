namespace CsvLite.Sql.Contexts;

public interface IContext
{
    IContext? Parent { get; }
}