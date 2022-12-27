using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Attributes;

public interface IAttributeDefinitionNode
{
    IEnumerable<IAttribute> EvaluateAttributes(IRelationEvaluateContext context);

    IEnumerable<IValue> EvaluateValues(IExpressionEvaluateContext context);
}
