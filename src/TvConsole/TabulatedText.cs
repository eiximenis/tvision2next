using System.Globalization;
using System.Text;
using Wcwidth;

namespace Tvision2.Console;

public class TabulatedText
{
    private readonly List<TabulatedTextEntry> _entries = [];

    public IEnumerable<TabulatedTextEntry> Entries => _entries;

    public TabulatedTextEntry AddEntry(params object[] values)
    {
        var line = new TabulatedTextEntry();
        _entries.Add(line);
        foreach (var value in values)
        {
            line.AddValue(value);
        }

        return line;
    }

    public TabulatedTextEntry AddEntry<T>(Func<T, string> formatter, params T[] values)
    {
        var entry = new TabulatedTextEntry();
        _entries.Add(entry);
        foreach (var value in values)
        {
            entry.AddValue(value, formatter);
        }

        return entry;
    }

    public int EntriesCount => _entries.Count;
    public int ColCount => _entries.Count > 0 ? _entries.Max(ttc => ttc.CellCount) : 0;
    public int ColLen(int colIdx) => _entries.Count > 0 ? _entries.Max(ttc => ttc.CelLen(colIdx)) : 0;
    public TabulatedTextCell ValueAt(int row, int col) => _entries[row].Cell(col);

    /// <summary>
    /// Gets the lines of the TabulatedText.
    /// Combining the different lines of different cells of the same entry.
    ///
    /// If we have a cell with two lines and another cell with three lines, the result will be a list of
    /// three lines:
    ///     - Line 1: Cell 1 Line 1 + Cell 2 Line 1
    ///     - Line 2: Cell 1 Line 2 + Cell 2 Line 2
    ///     - Line 3: Cell 2 Line 3
    /// </summary>
    public IEnumerable<TabulatedTextLine> GetLines()
    {
        var allLines = new List<TabulatedTextLine>();
        var someLine = true;
        foreach (var entry in _entries)
        {
            for (var lineIdx = 0; lineIdx < entry.LinesCount; lineIdx++)
            {
                var lineWidth = 0;
                var runes = new List<Rune>();
                var colIdx = 0;
                foreach (var cell in entry.Cells)
                {
                    var line = cell.LineAt(lineIdx);
                    var colLen = ColLen(colIdx) + 4;
                    if (!line.Empty)
                    {
                        runes.AddRange(line.Runes);
                    }

                    if (line.Length < colLen)
                    {
                        var emptyRunes = new Rune[colLen - line.Length];
                        Array.Fill(emptyRunes, new Rune(' '));
                        runes.AddRange(emptyRunes);
                    }
                    lineWidth += colLen;
                    colIdx++;
                }
                allLines.Add(new(runes, lineWidth));
            }

        }

        return allLines;
    }
}


public record TabulatedTextLine(IEnumerable<Rune> Runes, int Length)
{
    public bool Empty => !Runes.Any();
}
public class TabulatedTextCell
{

    private readonly List<TabulatedTextLine> _lines;


    public string RawValue { get; }
    public Type ValueType { get; }
    public int Height => _lines.Count;

    public int MaxLineLen { get; }

    public IEnumerable<TabulatedTextLine> Lines => _lines;

    public TabulatedTextLine LineAt(int idx) => idx < _lines.Count ? _lines[idx] : new TabulatedTextLine(Array.Empty<Rune>(), 0);

    public TabulatedTextCell(string value, Type valueType)
    {
        RawValue = value;
        ValueType = valueType;
        _lines = SplitLines(RawValue);
        MaxLineLen = _lines.Any() ? _lines.Max(l => l.Length) : 0;
    }

    private static List<TabulatedTextLine> SplitLines(string value)
    {
        var lines = new List<TabulatedTextLine>();
        var runes = value.EnumerateRunes();
        var line = new List<Rune>();
        var nl = new Rune('\n');


        foreach (var rune in runes)
        {
            var category = Rune.GetUnicodeCategory(rune);
            if (category == UnicodeCategory.LineSeparator || category == UnicodeCategory.ParagraphSeparator || rune == nl)
            {
                lines.Add(new(line, line.Sum(r => UnicodeCalculator.GetWidth(r))));
                line = new();
            }
            else
            {
                line.Add(rune);
            }
        }

        if (line.Any())
        {
            lines.Add(new(line, line.Sum(r => UnicodeCalculator.GetWidth(r))));
        }

        return lines;
    }

}
public class TabulatedTextEntry
{

    private readonly List<TabulatedTextCell> _values = [];

    public int LinesCount => _values.Max(c => c.Height);

    public void AddValue<T>(T value)
    {
        var strValue = value?.ToString() ?? "";
        _values.Add(new TabulatedTextCell(strValue, typeof(T)));
    }

    public void AddValue<T>(T value, Func<T, string?> formatter)
    {
        var strValue = formatter(value) ?? "";
        _values.Add(new TabulatedTextCell(strValue, typeof(T)));
    }

    public int CellCount => _values.Count;
    public int CelLen(int colIdx) => colIdx < _values.Count ? _values[colIdx].MaxLineLen : 0;
    public int CellHeight(int colIdx) => colIdx < _values.Count ? _values[colIdx].Height : 0;

    public IEnumerable<TabulatedTextCell> Cells => _values;
    public TabulatedTextCell Cell(int colIdx) => _values[colIdx];
}
