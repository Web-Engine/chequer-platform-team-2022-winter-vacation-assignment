using CsvLite.Models.Relations;
using CsvLite.Sql.Utilities;

namespace CsvLite.Cli;

public class RelationPresenter
{
    private readonly TextWriter _writer;

    public RelationPresenter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Show(IRelation relation)
    {
        var columnsCount = relation.Attributes.Count;

        var columnWidths = Enumerable.Range(0, columnsCount)
            .Select(index => MeasureColumn(relation, index))
            .ToArray();

        PrintHorizontalLine(columnWidths);
        PrintValues(
            columnWidths.Zip(
                relation.Attributes.Select(attribute => attribute.Name.Value)
            )
        );
        PrintHorizontalLine(columnWidths);

        foreach (var record in relation.Records)
        {
            PrintValues(
                columnWidths.Zip(
                    record.Select(value => value.AsString().Value)
                )
            );
            
            PrintHorizontalLine(columnWidths);
        }
    }

    private int MeasureColumn(IRelation relation, int index)
    {
        return relation.Records
            .Select(record => record[index].AsString().Value)
            .Append(relation.Attributes[index].Name.Value)
            .Select(value => value.Length)
            .Max();
    }

    private void PrintHorizontalLine(IEnumerable<int> columnWidths)
    {
        _writer.Write('+');

        foreach (var columnWidth in columnWidths)
        {
            _writer.Write('-');

            for (var i = 0; i < columnWidth; i++)
                _writer.Write('-');

            _writer.Write("-+");
        }
        
        _writer.WriteLine();
    }

    private void PrintValues(IEnumerable<(int ColumnWidth, string Value)> values)
    {
        _writer.Write('|');

        foreach (var (columnWidth, value) in values)
        {
            _writer.Write(' ');
            _writer.Write(value.PadRight(columnWidth, ' '));
            _writer.Write(" |");
        }
        
        _writer.WriteLine();
    }
}