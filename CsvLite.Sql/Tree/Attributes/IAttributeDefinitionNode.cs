using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Tree.Attributes;

public interface IAttributeDefinitionNode : INode
{
    IEnumerable<IAttribute> EvaluateAttributes(IRelationContext context);

    IEnumerable<IValue> EvaluateValues(IRecordContext context);
}
