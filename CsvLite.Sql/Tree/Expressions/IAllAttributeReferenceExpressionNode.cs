using CsvLite.Models.Attributes;
using CsvLite.Sql.Models.Attributes;

namespace CsvLite.Sql.Tree.Expressions;

public interface IAllAttributeReferenceExpressionNode : IAttributeReferenceExpressionNode
{
    IAttributeReference IAttributeReferenceExpressionNode.AttributeReference => AttributeReference;

    new AllAttributeReference AttributeReference { get; }
}