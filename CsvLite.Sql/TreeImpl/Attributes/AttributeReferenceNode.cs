using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Attributes;
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

            var attributes = FindAttributeIndexesInternal(context)
                .Select(index => context.Attributes[index])
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

            var indexes = FindAttributeIndexesInternal(context).ToList();

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

            var values = FindAttributeIndexesInternal(context)
                .Select(index => context.Record[index])
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

    private IEnumerable<int> FindAttributeIndexesInternal(IRelationContext context)
    {
        return context.AttributeIdentifiers
            .Select((identifier, index) => (identifier, index))
            .Where(x => Reference.IsReferencing(x.identifier))
            .Select(x => x.index);
    }
}