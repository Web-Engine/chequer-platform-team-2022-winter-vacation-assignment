using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class EmptyRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IContext context)
    {
        return Enumerable.Empty<IAttribute>();
    }

    public IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        return Enumerable.Empty<IRecord>();
    }
}