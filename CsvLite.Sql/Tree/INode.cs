namespace CsvLite.Sql.Tree;

public interface INode
{
    IEnumerable<INodeValue> Children { get; }
}