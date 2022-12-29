using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions;

public sealed class ExplicitAttributeReferenceExpressionNode : IExplicitAttributeReferenceExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public ExplicitAttributeReference AttributeReference { get; }

    public ExplicitAttributeReferenceExpressionNode(ExplicitAttributeReference attributeReference)
    {
        AttributeReference = attributeReference;
    }

    public IValue Evaluate(IRecordContext context)
    {
        IContext? loopContext = context;
        
        while (loopContext is not null)
        {
            if (loopContext is not IRecordContext recordContext)
            {
                loopContext = loopContext.Parent;
                continue;
            }

            var attributes = recordContext.Attributes
                .FindAttributes(AttributeReference)
                .ToList();

            if (attributes.Count > 1)
                throw new InvalidOperationException($"Attribute reference ambiguous {AttributeReference}");

            if (attributes.Count == 0)
                continue;

            var attributeIndex = attributes.First().Index;
            return recordContext.Record[attributeIndex];
        }

        throw new InvalidOperationException($"Cannot resolve attribute {AttributeReference}");
    }
}