using CsvLite.Models.Relations;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Models.Relations;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.Evaluators;

public class RelationEvaluator : IRelationEvaluator
{
    private readonly RelationEvaluateContext _context;

    public RelationEvaluator(IRelationProvider relationProvider)
        : this(
            new RelationEvaluateContext(relationProvider, new EmptyRelation())
        )
    {
    }

    public RelationEvaluator(RelationEvaluateContext context)
    {
        _context = context;
    }

    public IRelation Evaluate(IRelationNode node)
    {
        return node switch
        {
            IReferenceRelationNode referenceNode => EvaluateReferenceNode(referenceNode),
            IInheritRelationNode inheritNode => EvaluateInheritNode(inheritNode),
            IBinaryRelationNode complexNode => EvaluateBinaryNode(complexNode),

            _ => throw new InvalidOperationException($"Cannot resolve relation with {node.GetType()}")
        };
    }

    private IRelation EvaluateReferenceNode(IReferenceRelationNode node)
    {
        return _context.RelationProvider.GetRelation(node.Identifier);
    }

    private IRelation EvaluateInheritNode(IInheritRelationNode node)
    {
        var baseRelation = node.BaseRelationNode is { } baseRelationNode
            ? Evaluate(baseRelationNode)
            : new EmptyRelation();

        var subContext = _context.CreateSubContext(baseRelation);

        return node.Evaluate(subContext);
    }

    private IRelation EvaluateBinaryNode(IBinaryRelationNode node)
    {
        var baseRelation1 = Evaluate(node.BaseRelationNode1);
        var baseRelation2 = Evaluate(node.BaseRelationNode2);

        var joinedRelation = node.Combine(baseRelation1, baseRelation2);

        var subContext = _context.CreateSubContext(joinedRelation);

        return node.Evaluate(subContext);
    }
}