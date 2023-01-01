using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Tree.Relations;

public interface IRelationNode : INode
{
    IEnumerable<IAttribute> EvaluateAttributes(IContext context);

    IEnumerable<IRecord> EvaluateRecords(IRelationContext context);
    
    // IRelationContext Evaluate(IContext context);
}
