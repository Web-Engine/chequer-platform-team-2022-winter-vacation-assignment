using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ReferenceRelationNode : IRelationNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield break; }
    }

    public Identifier Identifier { get; }

    public ReferenceRelationNode(Identifier identifier)
    {
        Identifier = identifier;
    }

    public IRelation Evaluate(IRootContext context)
    {
        return context.GetPhysicalRelation(Identifier);
    }
}