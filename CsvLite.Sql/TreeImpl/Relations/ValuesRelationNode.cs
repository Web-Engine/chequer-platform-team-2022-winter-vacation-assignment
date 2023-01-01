using CsvLite.Models.Attributes;
using CsvLite.Models.Domains;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ValuesRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children => ExpressionNodes;

    public List<NodeValue<ITupleExpressionNode>> ExpressionNodes { get; set; }

    public ValuesRelationNode(IEnumerable<ITupleExpressionNode> expressionNodes)
    {
        ExpressionNodes = expressionNodes.Select(node => node.ToNodeValue()).ToList();
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IContext context)
    {
        IRelationContext relationContext = new AnonymousRelationContext(context, new EmptyRelation());

        if (ExpressionNodes.Count == 0)
            return Enumerable.Empty<IAttribute>();

        var tupleDomain = ExpressionNodes
            .Select(node => node.Value.EvaluateDomain(relationContext))
            .Aggregate((domain1, domain2) =>
            {
                if (!domain1.Domains.SequenceEqual(domain2.Domains))
                    throw new InvalidOperationException("VALUES domain should be same");

                return domain1;
            });

        var domains = tupleDomain.Domains
            .Select(domain =>
            {
                if (domain is not IPrimitiveDomain primitiveDomain)
                    throw new InvalidOperationException("VALUES cannot have non-primitive value");

                return primitiveDomain;
            })
            .ToList();

        return domains.Select(domain => new DefaultAttribute(Identifier.Empty, domain));
    }

    public IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        IRecordContext recordContext = new RecordContext(context, new DefaultRecord());

        return ExpressionNodes.Select(node =>
        {
            var tupleValue = node.Value.EvaluateValue(recordContext);

            return new DefaultRecord(tupleValue);
        });
    }
}