using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Models.Records;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.TreeImpl.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class AggregateRelationNode : BaseInheritRelationNode
{
    public override IEnumerable<INodeValue> Children
    {
        get
        {
            foreach (var value in base.Children)
            {
                yield return value;
            }

            yield return ExpressionNode;
        }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    private readonly IEnumerable<ExplicitAttributeReference> _attributeReferences;

    public AggregateRelationNode(IRelationNode baseRelationNode) : base(baseRelationNode)
    {
        _attributeReferences = Enumerable.Empty<ExplicitAttributeReference>();

        ExpressionNode = new LiteralExpressionNode(true).ToNodeValue<IExpressionNode>();
    }

    public AggregateRelationNode(IRelationNode baseRelationNode,
        IEnumerable<ExplicitAttributeReference> attributeReferences) : base(baseRelationNode)
    {
        _attributeReferences = attributeReferences;

        ExpressionNode = new TupleExpressionNode(
            _attributeReferences.Select(reference => new ExplicitAttributeReferenceExpressionNode(reference))
        ).ToNodeValue<IExpressionNode>();
    }

    protected override IRelation Evaluate(IRelationContext context)
    {
        var nonAggregateAttributeIndexes = _attributeReferences
            .SelectMany(reference => context.Attributes.FindAttributes(reference))
            .Select(x => x.Index)
            .ToHashSet();

        return new InheritRelation(
            context,
            
            records: context.Records
                .GroupBy(record =>
                {
                    var recordContext = context.CreateRecordContext(record);

                    return ExpressionNode.Evaluate(recordContext);
                })
                .Select(group =>
                    new AggregateRecord(group, nonAggregateAttributeIndexes)
                )
        );
    }
}