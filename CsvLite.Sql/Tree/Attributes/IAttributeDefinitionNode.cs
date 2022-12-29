using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Attributes;

public interface IAttributeDefinitionNode : INode
{
    IEnumerable<IAttribute> EvaluateAttributes(IRelationContext context);

    IEnumerable<IValue> EvaluateValues(IRecordContext context);
}
