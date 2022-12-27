using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.Evaluators;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IExpressionEvaluateContext _context;

    public ExpressionEvaluator(IExpressionEvaluateContext context)
    {
        _context = context;
    }

    public IValue Evaluate(IExpressionNode node)
    {
        return node switch
        {
            IAttributeReferenceExpressionNode referenceNode => EvaluateReferenceNode(_context, referenceNode),
            IEvaluateExpressionNode unaryNode => EvaluateExpressionNode(_context, unaryNode),

            _ => throw new InvalidOperationException($"Cannot resolve expression {node.GetType()}")
        };
    }

    private IValue EvaluateReferenceNode(IExpressionEvaluateContext context, IAttributeReferenceExpressionNode node)
    {
        return node switch
        {
            IAllAttributeReferenceExpressionNode allNode => EvaluateAllReferenceNode(context, allNode),
            IExplicitAttributeReferenceExpressionNode explicitNode => EvaluateExplicitReferenceNode(context, explicitNode),
            
            _ => throw new InvalidOperationException($"Unknown AttributeReferenceExpressionNode {node.GetType()}")
        };
    }

    private IValue EvaluateAllReferenceNode(IExpressionEvaluateContext context, IAllAttributeReferenceExpressionNode node)
    {
        while (true)
        {
            var attributes = context.Relation.Attributes
                .FindAttributes(node.AttributeReference)
                .ToList();

            if (attributes.Count != 0)
            {
                var values = attributes.Select(x => context.Record[x.Index]).ToList();

                return new TupleValue(values);
            }

            if (context.Parent is {Parent: { } parentContext})
            {
                context = parentContext;
                continue;
            }

            throw new InvalidOperationException($"Cannot resolve attribute {node.AttributeReference}");
        }
    }

    private IValue EvaluateExplicitReferenceNode(
        IExpressionEvaluateContext context,
        IExplicitAttributeReferenceExpressionNode node
    )
    {
        while (true)
        {
            var attributes = context.Relation.Attributes
                .FindAttributes(node.AttributeReference)
                .ToList();

            if (attributes.Count > 1)
                throw new InvalidOperationException($"Attribute reference ambiguous {node.AttributeReference}");

            if (attributes.Count == 1)
            {
                var attributeIndex = attributes.First().Index;

                return context.Record[attributeIndex];
            }

            if (context.Parent is {Parent: { } parentContext})
            {
                context = parentContext;
                continue;
            }

            throw new InvalidOperationException($"Cannot resolve attribute {node.AttributeReference}");
        }
    }


    private IValue EvaluateExpressionNode(IExpressionEvaluateContext context, IEvaluateExpressionNode node)
    {
        return node.Evaluate(context);
    }
}