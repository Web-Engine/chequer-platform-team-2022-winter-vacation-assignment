using CsvLite.IO.Csv;
using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
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

    protected override IRelation Evaluate(IRelationContext context)
    {
        var newAttributes = _attributeDefinitions
            .SelectMany(definitionNode => definitionNode.Value.EvaluateAttributes(context));

        var attributes = new DefaultAttributeList(newAttributes);

        var newRecords = context.Relation.Records
            .Select(record =>
            {
                var recordContext = context.CreateRecordContext(record);

                var values = _attributeDefinitions.SelectMany(definitionNode =>
                    definitionNode.Value.EvaluateValues(recordContext)
                );

                return new DefaultRecord(values);
            });


        return new DefaultRelation(
            attributes,
            newRecords
        );
    }
}