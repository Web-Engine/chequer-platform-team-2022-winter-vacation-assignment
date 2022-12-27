using CsvLite.Models.Attributes;
using CsvLite.Sql.Models.Attributes;

namespace CsvLite.Sql.Tree.Expressions;

public interface IExplicitAttributeReferenceExpressionNode : IAttributeReferenceExpressionNode
{
    IAttributeReference IAttributeReferenceExpressionNode.AttributeReference => AttributeReference;

    new ExplicitAttributeReference AttributeReference { get; }
}