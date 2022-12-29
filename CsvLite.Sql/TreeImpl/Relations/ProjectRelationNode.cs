using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.RelationContexts;
using CsvLite.Sql.Tree;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ProjectRelationNode : BaseInheritRelationNode
{
    public override IEnumerable<INodeValue> Children
    {
        get
        {
            foreach (var node in base.Children)
            {
                yield return node;
            }

            foreach (var node in _attributeDefinitions)
            {
                yield return node;
            }
        }
    }

    private readonly List<NodeValue<IAttributeDefinitionNode>> _attributeDefinitions;

    public ProjectRelationNode(
        IRelationNode relationNode,
        IReadOnlyList<IAttributeDefinitionNode> attributeDefinitions
    ) : base(relationNode)
    {
        _attributeDefinitions = attributeDefinitions
            .Select(node => node.ToNodeValue())
            .ToList();
    }

    protected override IRelationContext Resolve(IRootContext rootContext)
    {
        var context = base.Resolve(rootContext);

        var attributes = _attributeDefinitions
            .SelectMany(definitionNode => definitionNode.Value.EvaluateAttributes(context));

        var newRecords = context.Relation.Records
            .Select(record =>
            {
                var recordContext = new RecordContext(context, record);

                var values = _attributeDefinitions.SelectMany(definitionNode =>
                    definitionNode.Value.EvaluateValues(recordContext)
                );

                return new DefaultRecord(values);
            });

        var relation = new DefaultRelation(attributes, newRecords);

        return new AnonymousRelationContext(rootContext, relation);
    }
}