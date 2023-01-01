using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts.Records;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Tree.Attributes;

public interface IExplicitAttributeDefinitionNode : IAttributeDefinitionNode
{
    IEnumerable<IAttribute> IAttributeDefinitionNode.EvaluateAttributes(IRelationContext context)
    {
        yield return EvaluateAttribute(context);
    }

    IEnumerable<IValue> IAttributeDefinitionNode.EvaluateValues(IRecordContext context)
    {
        yield return EvaluateValue(context);
    }

    IAttribute EvaluateAttribute(IRelationContext context);

    IValue EvaluateValue(IRecordContext context);
}