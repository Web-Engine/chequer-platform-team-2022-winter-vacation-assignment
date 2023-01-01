using System.Collections;

namespace CsvLite.Models.Identifiers;

public sealed class QualifiedIdentifier : IEnumerable<Identifier>
{
    public Identifier[] Identifiers { get; }

    public int Level => Identifiers.Length;

    public Identifier this[int index] => Identifiers[index];

    public QualifiedIdentifier(params Identifier[] identifiers)
    {
        Identifiers = identifiers;
    }

    public QualifiedIdentifier(params string[] identifiers)
    {
        Identifiers = identifiers
            .Select(value => new Identifier(value))
            .ToArray();
    }

    public static QualifiedIdentifier Create(Identifier? relationIdentifier, Identifier attributeIdentifier)
    {
        if (relationIdentifier is null)
            return new QualifiedIdentifier(attributeIdentifier);

        return new QualifiedIdentifier(relationIdentifier, attributeIdentifier);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<Identifier> GetEnumerator()
    {
        return Identifiers.AsEnumerable().GetEnumerator();
    }

    public override string ToString()
    {
        return string.Join(
            ".",
            Identifiers.Select(
                identifier => identifier.ToString(escaped: true)
            )
        );
    }

    public override bool Equals(object? obj)
    {
        if (obj is not QualifiedIdentifier qualifiedIdentifier) return false;

        return Identifiers.SequenceEqual(qualifiedIdentifier.Identifiers);
    }

    public override int GetHashCode()
    {
        return Identifiers.Select(x => x.GetHashCode()).Sum();
    }
}