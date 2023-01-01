using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Attributes;

public class ExpressionAttributeDefinitionNode : IExplicitAttributeDefinitionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return ExpressionNode; }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    private readonly Identifier _name;

    public ExpressionAttributeDefinitionNode(IExpressionNode expressionNode, Identifier name)
    {
        ExpressionNode = expressionNode.ToNodeValue();
        _name = name;
    }

    public IAttribute EvaluateAttribute(IRelationContext context)
    {
        var domain = ExpressionNode.Value.EvaluateDomain(context);

        return new DefaultAttribute(_name, domain);
    }

    public IValue EvaluateValue(IRecordContext context)
    {
        return ExpressionNode.Value.EvaluateValue(context);
    }
}