using CsvLite.Models.Attributes;
using CsvLite.Models.Values;
using CsvLite.Sql.Contexts;

namespace CsvLite.Sql.Tree.Attributes;

public interface IAttributeReferenceNode : INode
{
    IAttributeReference Reference { get; }
    
    IEnumerable<IAttribute> GetAttributes(IRelationContext context, out IRelationContext found);
    
    IEnumerable<int> GetAttributeIndexes(IRelationContext context, out IRelationContext found);
    
    IEnumerable<IValue> GetValues(IRecordContext context, out IRecordContext found);
}
