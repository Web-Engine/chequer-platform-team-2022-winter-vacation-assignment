using System.Text;

namespace CsvLite.Utilities;

public static class StringUtility
{
    public static string Unescape(ReadOnlySpan<char> str, ReadOnlySpan<char> escapedQuote, char quote)
    {
        var builder = new StringBuilder();

        str = str[1..^1];

        while (!str.IsEmpty)
        {
            if (str.StartsWith(escapedQuote))
            {
                builder.Append(quote);
                str = str[escapedQuote.Length..];
                continue;
            }

            builder.Append(str[0]);
            str = str[1..];
        }

        return builder.ToString();
    }

    public static string Escape(ReadOnlySpan<char> str, char quote, ReadOnlySpan<char> escapedQuote)
    {
        var builder = new StringBuilder();
        builder.Append(quote);

        while (!str.IsEmpty)
        {
            if (str[0] == quote)
                builder.Append(escapedQuote);
            else
                builder.Append(str[0]);
            
            str = str[1..];
        }

        builder.Append(quote);

        return builder.ToString();
    }
    
    // public static string Start
}