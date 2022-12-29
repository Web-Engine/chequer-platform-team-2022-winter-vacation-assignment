using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.RelationContexts;
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

    public IRelationContext Evaluate(IRootContext context)
    {
        return new AnonymousRelationContext(context, new EmptyRelation());
    }
}