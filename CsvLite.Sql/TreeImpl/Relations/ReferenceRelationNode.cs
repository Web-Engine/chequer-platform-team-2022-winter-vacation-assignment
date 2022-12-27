using CsvLite.Models.Identifiers;
using CsvLite.Sql.Tree.Relations;

namespace CsvLite.Sql.TreeImpl.Relations;

public class ReferenceRelationNode : IReferenceRelationNode
{
    public Identifier Identifier { get; }

    public ReferenceRelationNode(Identifier identifier)
    {
        Identifier = identifier;
    }
}