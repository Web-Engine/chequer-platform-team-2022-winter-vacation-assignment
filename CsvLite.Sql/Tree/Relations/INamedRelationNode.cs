using CsvLite.Models.Attributes;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Records;
using CsvLite.Sql.Contexts;
using CsvLite.Sql.Contexts.Relations;

namespace CsvLite.Sql.Tree.Relations;

public interface INamedRelationNode : IRelationNode
{
    Identifier Identifier { get; }
}
