using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions;

public sealed class ExplicitAttributeReferenceExpressionNode : IExplicitAttributeReferenceExpressionNode
{
    public ExplicitAttributeReference AttributeReference { get; }

    public ExplicitAttributeReferenceExpressionNode(ExplicitAttributeReference attributeReference)
    {
        AttributeReference = attributeReference;
    }
}