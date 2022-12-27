using CsvLite.Models.Identifiers;

namespace CsvLite.Sql.Tree.Relations;

public interface IReferenceRelationNode : IRelationNode
{
    Identifier Identifier { get; }
}
