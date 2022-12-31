using CsvLite.Models.Relations;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.TreeImpl;

namespace CsvLite.Sql.Utilities;

public static class NodeExtension
{
    public static bool ContainsRecursive(
        this INode node,
        Func<INode, bool> predicate,
        Func<INode, bool>? nodeFilter = null)
    {
        foreach (var child in node.Children)
        {
            if (!nodeFilter?.Invoke(child.Node) ?? true) continue;
            
            if (predicate(child.Node)) return true;

            if (ContainsRecursive(child.Node, predicate, nodeFilter))
                return true;
        }

        return false;
    }

    public static NodeValue<TNode> ToNodeValue<TNode>(this TNode node) where TNode : class, INode
    {
        return new NodeValue<TNode>(node);
    }

    public static IValue Evaluate(this NodeValue<IExpressionNode> node, IRecordContext context)
    {
        return node.Value.Evaluate(context);
    }
    
    public static IRelationContext Evaluate(this NodeValue<IRelationNode> node, IRelationContext context)
    {
        return node.Value.Evaluate(context);
    }
}