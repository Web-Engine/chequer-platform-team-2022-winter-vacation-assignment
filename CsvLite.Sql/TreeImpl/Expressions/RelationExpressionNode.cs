using CsvLite.Models.Domains;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Expressions;

public class RelationExpressionNode : IExpressionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return RelationNode; }
    }

    public NodeValue<IRelationNode> RelationNode { get; }

    public RelationExpressionNode(IRelationNode relationNode)
    {
        RelationNode = relationNode.ToNodeValue();
    }

    public IDomain EvaluateDomain(IRelationContext context)
    {
        var attributes = RelationNode.Value.EvaluateAttributes(context);

        return new RelationDomain(attributes);
    }

    public IValue EvaluateValue(IRecordContext context)
    {
        var relationContext = RelationNode.Evaluate(context);

        return new RelationValue(relationContext.Relation);
    }
}