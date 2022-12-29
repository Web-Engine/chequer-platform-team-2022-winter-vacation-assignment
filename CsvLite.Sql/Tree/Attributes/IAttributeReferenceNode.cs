using CsvLite.Models.Attributes;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Attributes;

public interface IAttributeReferenceNode
{
    IEnumerable<IAttribute> Evaluate(IRelationContext context);
}
