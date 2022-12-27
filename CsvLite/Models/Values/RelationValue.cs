using CsvLite.Models.Relations;

namespace CsvLite.Models.Values;

public sealed class RelationValue : IValue
{
    public IRelation Relation { get; }

    public RelationValue(IRelation relation)
    {
        Relation = relation;
    }

    public int CompareTo(IValue? other)
    {
        throw new InvalidOperationException("Cannot compare relation");
    }

    public bool Equals(IValue? other)
    {
        throw new InvalidOperationException("Cannot compare relation");
    }
}