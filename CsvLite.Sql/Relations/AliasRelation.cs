using CsvLite.Models.Attributes;
using CsvLite.Models.Relations;
using CsvLite.Models.Tuples;
using CsvLite.Sql.Attributes;

namespace CsvLite.Sql.Relations;

public class AliasRelation : IRelation
{
    public IEnumerable<IAttribute> Attributes
    {
        get
        {
            foreach (var attribute in _relation.Attributes)
            {
                yield return new AliasAttribute(attribute, new[] {_alias, attribute.Name});
            }
        }
    }

    public IEnumerable<ITuple> Tuples => _relation.Tuples;

    private readonly IRelation _relation;
    private readonly string _alias;

    public AliasRelation(IRelation relation, string alias)
    {
        _relation = relation;
        _alias = alias;
    }
}
