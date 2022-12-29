using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions;

public sealed class AllAttributeReferenceExpressionNode : IAllAttributeReferenceExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public AllAttributeReference AttributeReference { get; }

    public AllAttributeReferenceExpressionNode(AllAttributeReference attributeReference)
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

            if (attributes.Count == 0) continue;
            
            var values = attributes.Select(
                x => recordContext.Record[x.Index]
            );
            
            return new TupleValue(values);
        }

        throw new InvalidOperationException($"Cannot resolve attribute {AttributeReference}");
    }
}