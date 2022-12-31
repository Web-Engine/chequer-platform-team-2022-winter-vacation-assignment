using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Models.Results.Actions;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Actions;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Actions;

public class SelectActionNode : IActionNode
{
    public IEnumerable<INodeValue> Children
    {
        get { yield return RelationNode; }
    }

    public NodeValue<IRelationNode> RelationNode { get; }

    public SelectActionNode(IRelationNode relationNode)
    {
        RelationNode = relationNode.ToNodeValue();
    }

    public IActionResult Execute(IContext context)
    {
        var relationContext = RelationNode.Value.Evaluate(context);

        return new SelectActionResult(
            new InheritRelation(
                relationContext,
                records: relationContext.Records.ToList()
            )
        );
    }

    private class SelectActionResult : IRelationActionResult
    {
        public IRelation Relation { get; }

        public SelectActionResult(IRelation relation)
        {
            Relation = relation;
        }
    }
}