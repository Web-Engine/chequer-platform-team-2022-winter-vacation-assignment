using CsvLite.IO.Csv;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ConditionRelationNode : IInheritRelationNode
{
    public IRelationNode? BaseRelationNode { get; }
    private readonly IExpressionNode _conditionExpressionNode;

    public ConditionRelationNode(IRelationNode? baseRelationNode, IExpressionNode conditionExpressionNode)
    {
        BaseRelationNode = baseRelationNode;
        _conditionExpressionNode = conditionExpressionNode;
    }

    public IRelation Evaluate(IRelationEvaluateContext context)
    {
        return new DefaultRelation(
            context.Relation.Attributes,
            context.Relation.Records.Where(record =>
            {
                var expressionContext = context.CreateExpressionEvaluateContext(record);
                var evaluator = expressionContext.CreateExpressionEvaluator();
                
                var condition = evaluator.Evaluate(_conditionExpressionNode).AsBoolean();
                return condition.Value;
            }).ToList()
        );
    }
}