using CsvLite.Models.Attributes;

namespace CsvLite.Sql.Tree.Expressions;

public interface IAttributeReferenceExpressionNode : IExpressionNode
{
    IAttributeReference AttributeReference { get; }
}