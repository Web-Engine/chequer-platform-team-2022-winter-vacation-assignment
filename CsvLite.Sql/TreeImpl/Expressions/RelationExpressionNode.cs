using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class RelationExpressionNode : IEvaluateExpressionNode
{
    private readonly IRelationNode _relationNode;

    public RelationExpressionNode(IRelationNode relationNode)
    {
        _relationNode = relationNode;
    }

    public IValue Evaluate(IExpressionEvaluateContext context)
    {
        var evaluator = context.CreateRelationEvaluator();

        var relation = evaluator.Evaluate(_relationNode);

        return new RelationValue(relation);
    }
}