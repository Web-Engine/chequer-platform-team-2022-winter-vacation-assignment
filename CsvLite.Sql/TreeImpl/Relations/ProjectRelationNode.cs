using CsvLite.IO.Csv;
using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Attributes;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ProjectRelationNode : IInheritRelationNode
{
    private readonly IReadOnlyList<IAttributeDefinitionNode> _attributeDefinitions;

    public IRelationNode? BaseRelationNode { get; }

    public ProjectRelationNode(
        IRelationNode? baseRelationNode,
        IReadOnlyList<IAttributeDefinitionNode> attributeDefinitions
    )
    {
        _attributeDefinitions = attributeDefinitions;
        BaseRelationNode = baseRelationNode;
    }

    public IRelation Evaluate(IRelationEvaluateContext context)
    {
        var newAttributes = _attributeDefinitions
            .SelectMany(definitionNode => definitionNode.EvaluateAttributes(context));

        var newRecords = context.Relation.Records
            .Select(record =>
            {
                var expressionContext = context.CreateExpressionEvaluateContext(record);
                var values = _attributeDefinitions.SelectMany(definitionNode => definitionNode.EvaluateValues(expressionContext));

                var newRecord = new DefaultRecord(values);

                return newRecord;
            })
            .ToList();

        var attributes = new DefaultAttributeList(newAttributes);

        return new DefaultRelation(
            attributes,
            newRecords
        );
    }
}