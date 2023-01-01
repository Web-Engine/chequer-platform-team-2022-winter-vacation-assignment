using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.TreeImpl.Expressions;
using CsvLite.Sql.TreeImpl.Relations;
using static CsvLite.Sql.TreeImpl.Expressions.BinaryBooleanAlgebraExpressionNode;

namespace CsvLite.Sql.Optimizers;

public sealed class RuleBasedOptimizer : ISqlOptimizer
{
    public TNode Optimize<TNode>(TNode node) where TNode : class, INode
    {
        return OptimizeInternal(node) as TNode ?? throw new InvalidOperationException();
    }

    private INode OptimizeInternal(INode node)
    {
        OptimizeChildren(node);

        return node switch
        {
            IRelationNode relationNode => OptimizeRelationInternal(relationNode),
            IExpressionNode expressionNode => OptimizeExpressionInternal(expressionNode),

            _ => node
        };
    }

    private void OptimizeChildren(INode node)
    {
        foreach (var child in node.Children)
        {
            child.Node = OptimizeInternal(child.Node);
        }
    }

    private static IRelationNode OptimizeRelationInternal(IRelationNode node)
    {
        switch (node)
        {
            case ConditionRelationNode condition:
                return OptimizeConditionRelation(condition);
        }

        return node;
    }

    private static IRelationNode OptimizeConditionRelation(ConditionRelationNode node)
    {
        if (node is not {ExpressionNode.Value: LiteralExpressionNode literal})
            return node;

        var boolean = literal.Value.AsBoolean().Value;

        return boolean
            ? node.RelationNode.Value
            : new EmptyRelationNode();
    }

    private static IExpressionNode OptimizeExpressionInternal(IExpressionNode node)
    {
        return node switch
        {
            ComparisonExpressionNode comparison => OptimizeComparisonExpression(comparison),
            BinaryBooleanAlgebraExpressionNode binaryBooleanAlgebra
                => OptimizeBinaryBooleanAlgebraExpression(binaryBooleanAlgebra),
            UnaryBooleanAlgebraExpressionNode unaryBooleanAlgebra
                => OptimizeUnaryBooleanAlgebraExpression(unaryBooleanAlgebra),
            BinaryCalculateExpressionNode binaryCalculate => OptimizeBinaryCalculateExpression(binaryCalculate),

            _ => node
        };
    }

    private static IExpressionNode OptimizeComparisonExpression(ComparisonExpressionNode node)
    {
        if (
            node is not
            {
                ExpressionNode1.Value: LiteralExpressionNode literal1,
                ExpressionNode2.Value: LiteralExpressionNode literal2
            }
        )
            return node;

        var value = node.Evaluate(literal1.Value, literal2.Value);

        return new LiteralExpressionNode(value);
    }

    private static IExpressionNode OptimizeBinaryBooleanAlgebraExpression(BinaryBooleanAlgebraExpressionNode node)
    {
        if (node.Operator is BinaryBooleanAlgebraOperator.And)
        {
            if (node.ExpressionNode1.Value is LiteralExpressionNode literal1 &&
                literal1.Value.AsBoolean().Value == false)
                return new LiteralExpressionNode(false);

            if (node.ExpressionNode2.Value is LiteralExpressionNode literal2 &&
                literal2.Value.AsBoolean().Value == false)
                return new LiteralExpressionNode(false);
        }

        else if (node.Operator is BinaryBooleanAlgebraOperator.Or)
        {
            if (node.ExpressionNode1.Value is LiteralExpressionNode literal1 && literal1.Value.AsBoolean().Value)
                return new LiteralExpressionNode(true);

            if (node.ExpressionNode2.Value is LiteralExpressionNode literal2 && literal2.Value.AsBoolean().Value)
                return new LiteralExpressionNode(true);
        }

        return node;
    }

    private static IExpressionNode OptimizeUnaryBooleanAlgebraExpression(UnaryBooleanAlgebraExpressionNode node)
    {
        if (node is not {ExpressionNode.Value: LiteralExpressionNode literal})
            return node;

        var value = node.Evaluate(literal.Value);
        return new LiteralExpressionNode(value);
    }

    private static IExpressionNode OptimizeBinaryCalculateExpression(BinaryCalculateExpressionNode node)
    {
        if (
            node is not
            {
                ExpressionNode1.Value: LiteralExpressionNode literal1,
                ExpressionNode2.Value: LiteralExpressionNode literal2
            }
        )
        {
            return node;
        }

        var value = node.Evaluate(literal1.Value, literal2.Value);
        return new LiteralExpressionNode(value);
    }
}