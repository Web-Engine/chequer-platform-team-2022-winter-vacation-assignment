using CsvLite.Sql.Models.Attributes;
using CsvLite.Sql.Tree.Expressions;

namespace CsvLite.Sql.TreeImpl.Expressions;

public sealed class AllAttributeReferenceExpressionNode : IAllAttributeReferenceExpressionNode
{
    public AllAttributeReference AttributeReference { get; }

    public AllAttributeReferenceExpressionNode(AllAttributeReference attributeReference)
    {
        AttributeReference = attributeReference;
    }
}