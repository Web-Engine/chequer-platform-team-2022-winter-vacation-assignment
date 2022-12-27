using System.Text;
using CsvLite.Utilities;

namespace CsvLite.Models.Identifiers;

public sealed class Identifier
{
    public static Identifier Empty { get; } = new("");

    public string Value { get; }

    public Identifier(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public string ToString(bool escaped)
    {
        if (!escaped)
            return ToString();

        return Escape(Value);
    }

    public static string Unescape(ReadOnlySpan<char> text)
    {
        return StringUtility.Unescape(text, "\\\"", '"');
    }

    public static string Escape(ReadOnlySpan<char> text)
    {
        var builder = new StringBuilder();

        while (!text.IsEmpty)
        {
            switch (text[0])
            {
                case '"':
                    builder.Append("\\\"");
                    break;

                default:
                    builder.Append(text[0]);
                    break;
            }

            text = text[1..];
        }

        return builder.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Identifier identifier) return false;

        return Value.Equals(identifier.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}