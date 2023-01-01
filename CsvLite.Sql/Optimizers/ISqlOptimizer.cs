using CsvLite.Sql.Tree;

namespace CsvLite.Sql.Optimizers;

public interface ISqlOptimizer
{
    TNode Optimize<TNode>(TNode node) where TNode : class, INode;
}