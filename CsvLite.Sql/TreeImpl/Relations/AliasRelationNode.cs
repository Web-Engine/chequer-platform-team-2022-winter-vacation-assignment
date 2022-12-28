using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class AliasRelationNode : IInheritRelationNode
{
    public IRelationNode BaseRelationNode { get; }

    private readonly Identifier _alias;

    public AliasRelationNode(IRelationNode baseRelationNode, Identifier alias)
    {
        BaseRelationNode = baseRelationNode;
        _alias = alias;
    }

    public IRelation Evaluate(IRelationEvaluateContext context)
    {
        var attributes = context.Relation.Attributes.Select(attribute => new DefaultAttribute(
            _alias,
            attribute.Name
        ));

        var attributeList = new DefaultAttributeList(attributes);

        return new InheritRelation(
            context.Relation,
            attributes: attributeList
        );
    }
}