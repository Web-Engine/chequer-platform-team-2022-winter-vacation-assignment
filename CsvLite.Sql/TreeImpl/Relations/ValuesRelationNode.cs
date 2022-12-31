using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Records;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ValuesRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children => ExpressionNodes;

    public List<NodeValue<IExpressionNode>> ExpressionNodes { get; set; }

    public ValuesRelationNode(IEnumerable<IExpressionNode> expressionNodes)
    {
        ExpressionNodes = expressionNodes.Select(node => node.ToNodeValue()).ToList();
    }

    public IRelationContext Evaluate(IContext context)
    {
        var emptyRelationContext = new AnonymousRelationContext(context, new EmptyRelation());
        var emptyRecordContext = new RecordContext(emptyRelationContext, DefaultRecord.Empty);

        var records = ExpressionNodes
            .Select(node => node.Evaluate(emptyRecordContext).AsTuple().ToRecord())
            .ToList();

        if (records.Count == 0)
            return emptyRelationContext;

        var firstRecord = records.First();

        var attributes = Enumerable.Range(0, firstRecord.Count)
            .Select(_ => DefaultAttribute.Empty);

        var relation = new DefaultRelation(
            attributes,
            records
        );

        return new AnonymousRelationContext(context, relation);
    }
}