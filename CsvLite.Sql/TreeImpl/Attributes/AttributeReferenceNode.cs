using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Utilities;

namespace CsvLite.Sql.TreeImpl.Attributes;

public class AttributeReferenceNode : IAttributeReferenceNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public IAttributeReference Reference { get; }

    public AttributeReferenceNode(IAttributeReference reference)
    {
        Reference = reference;
    }

    public IEnumerable<IAttribute> GetAttributes(IRelationContext initialContext, out IRelationContext foundContext)
    {
        IContext? loopContext = initialContext;

        while (loopContext is not null)
        {
            if (loopContext is not IRelationContext context)
            {
                loopContext = loopContext.Parent;
                continue;
            }

            var attributes = context.AttributeIdentifiers
                .WithIndex()
                .Where(x => Reference.IsReferencing(x.Value))
                .Select(x => context.Attributes[x.Index])
                .ToList();

            if (attributes.Count == 0)
            {
                loopContext = loopContext.Parent;
                continue;
            }

            foundContext = context;
            return attributes;
        }

        throw new InvalidOperationException($"Cannot resolve attribute using {Reference}");
    }

    public IEnumerable<int> GetAttributeIndexes(IRelationContext initialContext, out IRelationContext foundContext)
    {
        IContext? loopContext = initialContext;

        while (loopContext is not null)
        {
            if (loopContext is not IRelationContext context)
            {
                loopContext = loopContext.Parent;
                continue;
            }

            var indexes = initialContext.AttributeIdentifiers
                .WithIndex()
                .Where(x => Reference.IsReferencing(x.Value))
                .Select(x => x.Index)
                .ToList();


            if (indexes.Count == 0)
            {
                loopContext = loopContext.Parent;
                continue;
            }

            foundContext = context;
            return indexes;
        }

        throw new InvalidOperationException($"Cannot resolve attribute using {Reference}");
    }

    public IEnumerable<IValue> GetValues(IRecordContext initialContext, out IRecordContext found)
    {
        IContext? loopContext = initialContext;

        while (loopContext is not null)
        {
            if (loopContext is not IRecordContext context)
            {
                loopContext = loopContext.Parent;
                continue;
            }

            var values = initialContext.AttributeIdentifiers
                .WithIndex()
                .Where(x => Reference.IsReferencing(x.Value))
                .Select(x => context.Record[x.Index])
                .ToList();

            if (values.Count == 0)
            {
                loopContext = loopContext.Parent;
                continue;
            }

            found = context;
            return values;
        }

        throw new InvalidOperationException($"Cannot resolve attribute using {Reference}");
    }
}