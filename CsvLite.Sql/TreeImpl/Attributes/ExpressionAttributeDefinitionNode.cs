using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Expressions;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Attributes;

public class ExpressionAttributeDefinitionNode : IAttributeDefinitionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return ExpressionNode; }
    }

    public NodeValue<IExpressionNode> ExpressionNode { get; }

    private readonly Identifier _name;

    public ExpressionAttributeDefinitionNode(Identifier name, IExpressionNode expressionNode)
    {
        _name = name;
        ExpressionNode = expressionNode.ToNodeValue();
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IRelationContext context)
    {
        yield return new DefaultAttribute(_name);
    }

    public IEnumerable<IValue> EvaluateValues(IRecordContext context)
    {
        yield return ExpressionNode.Value.Evaluate(context);
    }
}