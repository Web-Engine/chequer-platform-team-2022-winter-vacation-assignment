using CsvLite.Models.Attributes;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class EmptyRowRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public IRelationContext Evaluate(IContext context)
    {
        return new AnonymousRelationContext(context, new EmptyRowRelation());
    }

    public IEnumerable<IAttribute> EvaluateAttributes(IContext context)
    {
        return Enumerable.Empty<IAttribute>();
    }

    public IEnumerable<IRecord> EvaluateRecords(IRelationContext context)
    {
        yield return new DefaultRecord();
    }
}