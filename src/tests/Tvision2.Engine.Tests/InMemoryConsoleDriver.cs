using System.Text;
using Tvision2.Core;
using Tvision2.Core.Console;

namespace Tvision2.Engine.Tests;

public class InMemoryConsoleDriver : IConsoleDriver
{
    private Rune[,] _runes;
    private CharacterAttribute[,] _attributes;
    
    private TvBounds Bounds { get; }
    
    public InMemoryConsoleDriver(TvBounds size)
    {
        Bounds = size;
        _runes = new Rune[Bounds.Width, Bounds.Height];
        _attributes = new CharacterAttribute[Bounds.Width, Bounds.Height];
    }
    
    
    public void WriteCharacterAt(int x, int y, Rune character, CharacterAttribute attribute)
    {
        _runes[x, y] = character;
        _attributes[x, y] = attribute;
    }

    public void WriteCharactersAt(int x, int y, int count, Rune character, CharacterAttribute attribute)
    {
        for (var idx = 0; idx < count; idx++)
        {
            WriteCharacterAt(x + idx, y, character, attribute);
        }
    }

    public void SetCursorAt(int x, int y)
    {
        
    }

    public void SetCursorVisibility(bool isVisible)
    {
        
    }

    public void ClearScreen(TvColor fore, TvColor back)
    {
        
    }

    public void SetWindowsSize(TvBounds value)
    {
    }

    public bool CanChangeWindowsSize { get; }
    
    // Methods to be used by tests

    public IEnumerable<Rune> GetRunesOfLine(int line, int startingCol=0, int length = -1 )
    {
        var limit = length == -1 ? Bounds.Width : startingCol + length;
        for (var col = startingCol; col < limit; col++)
        {
            yield return _runes[col, line];
        } 
    }

    public IEnumerable<char> GetCharsOfLine(int line, int startingCol = 0, int length = -1)
    {
        var limit = length == -1 ? Bounds.Width : startingCol + length;
        for (var col = startingCol; col < limit; col++)
        {
            var rune = _runes[col, line];
            if (rune.IsBmp) yield return (char)rune.Value;
        } 
    }
    
    
}