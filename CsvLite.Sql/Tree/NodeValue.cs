namespace CsvLite.Sql.Tree;

public sealed class NodeValue<TNode> : INodeValue where TNode : class, INode
{
    INode INodeValue.Node
    {
        get => Value;
        set => Value = value as TNode ?? throw new InvalidOperationException();
    }

    public TNode Value { get; set; }

    public NodeValue(TNode value)
    {
        Value = value;
    }
}