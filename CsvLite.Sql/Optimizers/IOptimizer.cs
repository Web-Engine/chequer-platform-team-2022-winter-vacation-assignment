using CsvLite.Sql.Tree;

namespace CsvLite.Sql.Optimizers;

public interface IOptimizer
{
    TNode Optimize<TNode>(TNode node) where TNode : class, INode;
}