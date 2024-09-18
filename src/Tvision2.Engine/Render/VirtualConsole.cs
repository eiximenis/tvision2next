using System.Diagnostics;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Wcwidth;

namespace Tvision2.Engine.Render;



public class ConsoleCharacterPair
{
    public enum DirtyStatus
    {
        Clean = 0,
        Dirty = 1
    }

    private ConsoleCharacter _current = ConsoleCharacter.Null;
    private ConsoleCharacter _new = ConsoleCharacter.Null;
    public ref ConsoleCharacter Current => ref _current;
    public ref ConsoleCharacter New => ref _new;

    public DirtyStatus Dirty { get; private set; }

    public void SetCurrent(ConsoleCharacter value) => _current = value;

    /// <summary>
    /// Sets the New value
    /// Returns a value indicating if the pair was clean before the operation and is clean after the operation
    /// (which means that the new was a no-op)
    ///
    /// After seting the new value, the Dirty Status can be either Clean or Dirty
    ///
    /// (true, Dirty = Clean)  => The pair was clean before the operation and is clean after the operation (no-op).
    /// (false, Dirty = Dirty) => The pair was dirty before the operation and is dirty after the operation (new value != current value).
    /// (false, Dirty = Clean) => The pair was dirty before the operation and is clean after the operation  (going from dirty to clean).
    /// (true, Dirty = Dirty)  => ** NOT POSSIBLE **
    /// </summary>
    public bool SetNew(ConsoleCharacter value)
    {
        var wasClean = Dirty == DirtyStatus.Clean;
        _new = value;
        Dirty = _new != _current ? DirtyStatus.Dirty : DirtyStatus.Clean;
        
        return wasClean && Dirty == DirtyStatus.Clean;
    }
    public void Flush()
    {
        _current = _new;
        Dirty = DirtyStatus.Clean;
    }
}

public class VirtualConsole
{

    private readonly ConsoleCharacterPair[] _buffer;
    private int _dirtyCount = 0;
    private readonly int[] _layerProcessedInCycle;
    private readonly TvColor _defaultBg;
    private int _currentLayerProcessing;

    public TvBounds Bounds { get; }

    public void DrawCharactersAt(char character, int count, TvPoint location, CharacterAttribute attr, in Viewzone cropzone) =>
        DrawRunesAt(new Rune(character), count, location, attr, in cropzone);

    public void DrawRunesAt(Rune rune, int count, TvPoint location, CharacterAttribute attr, in Viewzone cropzone)
    {
        if (!cropzone.ContainsLine(location.Y) || !cropzone.ContainsColumn(location.X))
        {
            return;
        }
        Span<Rune> runes = stackalloc Rune[count];
        runes.Fill(rune);
        DrawRunesAt(runes, location, attr, in cropzone);
    }

    public void DrawAt(string text, TvPoint location, CharacterAttribute attr, in Viewzone cropzone) => DrawRunesAt(text.AsSpan().EnumerateRunes(), location, attr, in cropzone);
    public void DrawAt(ReadOnlySpan<char> text, TvPoint location, CharacterAttribute attr, in Viewzone cropzone) => DrawRunesAt(text.EnumerateRunes(), location, attr, in cropzone);

    public void DrawRunesAt(ReadOnlySpan<Rune> runes, TvPoint location, CharacterAttribute attr, in Viewzone cropzone)
    {
        if (!cropzone.ContainsLine(location.Y) || !cropzone.ContainsColumn(location.X))
        {
            return;
        }

        var lineBuffer = _buffer.AsSpan(location.Y * Bounds.Width, Bounds.Width);
        var processedInCycle = _layerProcessedInCycle.AsSpan(location.Y * Bounds.Width, Bounds.Width);
        var startcol = location.X;
        var endcol = Math.Min(cropzone.BottomRight.X, Bounds.Width - 1);
        var idx = startcol;
        var enumerator = runes.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var rune = enumerator.Current;
            var newchar = new ConsoleCharacter(rune, attr);
            var runeWidth = UnicodeCalculator.GetWidth(rune);
            if (runeWidth > 0)
            {
                if (_currentLayerProcessing <= processedInCycle[idx])
                {
                    processedInCycle[idx] = _currentLayerProcessing;
                    if (runeWidth == 2)
                    {
                        processedInCycle[idx + 1] = _currentLayerProcessing;
                    }

                    var result = lineBuffer[idx].SetNew(newchar);

                    var deltaDirty = (result, lineBuffer[idx].Dirty) switch
                    {
                        (false, ConsoleCharacterPair.DirtyStatus.Dirty) => runeWidth,
                        (false, ConsoleCharacterPair.DirtyStatus.Clean) => -runeWidth,
                        _ => 0
                    };
                    if (runeWidth == 2)
                    {
                        lineBuffer[idx + 1].SetNew(ConsoleCharacter.Null);
                    }

                    _dirtyCount += deltaDirty;

                }
                idx += runeWidth;
                if (idx > endcol) { break; }
            }
        }

    }

    private void DrawRunesAt(SpanRuneEnumerator runes, TvPoint location, CharacterAttribute attr, in Viewzone cropzone)
    {
        if (!cropzone.ContainsLine(location.Y) || !cropzone.ContainsColumn(location.X))
        {
            return;
        }

        var lineBuffer = _buffer.AsSpan(location.Y * Bounds.Width, Bounds.Width);
        var processedInCycle = _layerProcessedInCycle.AsSpan(location.Y * Bounds.Width, Bounds.Width);
        var startcol = location.X;
        var endcol = Math.Min(cropzone.BottomRight.X, Bounds.Width - 1);
        var idx = startcol;
        while (runes.MoveNext())
        {
            var rune = runes.Current;
            var newchar = new ConsoleCharacter(rune, attr);
            var runeWidth = UnicodeCalculator.GetWidth(rune);
            if (runeWidth > 0)
            {
                if (_currentLayerProcessing <= processedInCycle[idx])
                {
                    processedInCycle[idx] = _currentLayerProcessing;
                    if (runeWidth == 2)
                    {
                        processedInCycle[idx + 1] = _currentLayerProcessing;
                    }

                    var result = lineBuffer[idx].SetNew(newchar);

                    var deltaDirty = (result, lineBuffer[idx].Dirty) switch
                    {
                        (false, ConsoleCharacterPair.DirtyStatus.Dirty) => runeWidth,
                        (false, ConsoleCharacterPair.DirtyStatus.Clean) => -runeWidth,
                        _ => 0
                    };
                    if (runeWidth == 2)
                    {
                        lineBuffer[idx + 1].SetNew(ConsoleCharacter.Null);
                    }

                    _dirtyCount += deltaDirty;

                }

                idx += runeWidth;
                if (idx > endcol)
                {
                    break;
                }
            }
        }
    }


    public void FillRect(TvPoint location, CharacterAttribute attr, in Viewzone cropzone)
    {
        if (!cropzone.ContainsLine(location.Y) || !cropzone.ContainsColumn(location.X))
        {
            return;
        }

        var startcol = location.X;
        var endcol = Math.Min(cropzone.BottomRight.X, Bounds.Width - 1);
        var startrow = location.Y;
        var endrow = Math.Min(cropzone.BottomRight.Y, Bounds.Height - 1);

        for (var crow = startrow; crow <= endrow; crow++)
        {
            var lineBuffer = _buffer.AsSpan(crow * Bounds.Width, Bounds.Width);
            var processedInCycle = _layerProcessedInCycle.AsSpan(crow * Bounds.Width, Bounds.Width);
            for (var ccol = startcol; ccol <= endcol; ccol++)
            {
                var newchar = new ConsoleCharacter(' ', attr);
                if (_currentLayerProcessing <= processedInCycle[ccol])
                {
                    processedInCycle[ccol] = _currentLayerProcessing;

                    var result = lineBuffer[ccol].SetNew(newchar);

                    var deltaDirty = (result, lineBuffer[ccol].Dirty) switch
                    {
                        (false, ConsoleCharacterPair.DirtyStatus.Dirty) => 1,
                        (false, ConsoleCharacterPair.DirtyStatus.Clean) => -1,
                        _ => 0
                    };
                    _dirtyCount += deltaDirty;

                }
            }
        }
    }

    public VirtualConsole(TvBounds bounds, TvColor defaultBack)
    {
        Bounds = bounds;
        _defaultBg = defaultBack;
        _buffer = new ConsoleCharacterPair[bounds.Length];
        _layerProcessedInCycle = new int[bounds.Length];
        Array.Fill(_layerProcessedInCycle, Int32.MaxValue);
        for (var idx = 0; idx < _buffer.Length; idx++)
        {
            _buffer[idx] = new ConsoleCharacterPair();
        }

        _currentLayerProcessing = 0;
    }

    public void StartNewLayer()
    {
        _currentLayerProcessing++;
    }
    public void Flush(IConsoleDriver consoleDriver)
    {
        _currentLayerProcessing = 0;
        Array.Fill(_layerProcessedInCycle, Int32.MaxValue);

        if (_dirtyCount <= 0)
        {
            _dirtyCount = 0;
            return;
        }

        _dirtyCount = 0;
        var height = Bounds.Height;
        var width = Bounds.Width;
        var idx = 0;
        for (var row = 0; row < height; row++)
        {
            var span = 1;
            var spanning = false;
            var spanCol = 0;
            for (var col = 0; col < width; col++)
            {
                var isDirty = _buffer[idx].Dirty == ConsoleCharacterPair.DirtyStatus.Dirty;
                if (isDirty)
                {
                    _buffer[idx].Flush();
                }

                ref var cc = ref _buffer[idx].Current;
                if (spanning || isDirty)
                {
                    var compareWithNext = col < width - 1;
                    var cnext = compareWithNext
                        // Next character is not still flushed, so it can be dirty. In this case we need to compare against New value (not Current)
                        ? _buffer[idx + 1].Dirty == ConsoleCharacterPair.DirtyStatus.Dirty ? _buffer[idx + 1].New : _buffer[idx + 1].Current
                        : ConsoleCharacter.Null;

                    if (compareWithNext && cc == cnext)
                    {
                        span++;
                        spanning = true;
                    }
                    else
                    {
                        consoleDriver.WriteCharactersAt(spanCol, row, span, cc.Character, cc.Attributes);
                        span = 1;
                        spanning = false;
                        spanCol = col + 1;
                    }
                }
                else
                {
                    spanCol++;
                }
                idx++;
            }
        }
    }
}
