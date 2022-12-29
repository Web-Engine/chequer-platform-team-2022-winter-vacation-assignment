using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
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

    public IRelation Evaluate(IRootContext context)
    {
        var relationContext = context.CreateRelationContext(new EmptyRelation());
        var recordContext = relationContext.CreateRecordContext(DefaultRecord.Empty);

        var records = ExpressionNodes
            .Select(node => node.Evaluate(recordContext).AsTuple().ToRecord())
            .ToList();

        if (records.Count == 0)
            return new EmptyRelation();

        var firstRecord = records.First();

        var attributes = new DefaultAttributeList(
            Enumerable.Range(0, firstRecord.Count)
                .Select(_ => DefaultAttribute.Empty)
        );

        return new DefaultRelation(
            attributes,
            records
        );
    }
}