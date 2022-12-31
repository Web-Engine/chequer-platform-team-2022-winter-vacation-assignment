using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class EmptyRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public IRelationContext Evaluate(IContext context)
    {
        return new AnonymousRelationContext(context, new EmptyRelation());
    }
}