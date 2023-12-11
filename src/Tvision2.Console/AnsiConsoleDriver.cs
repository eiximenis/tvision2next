using System.Text;
using Tvision2.Console.Colors;
using Tvision2.Core;
using Tvision2.Core.Console;

namespace Tvision2.Console;

public class AnsiConsoleDriver : IConsoleDriver
{
    private readonly AnsiColorManager _colorManager;
    private readonly AnsiConsoleOptions _options;
    
    public ConsoleOptions ConsoleOptions => _options;

    public AnsiConsoleDriver(AnsiConsoleOptions options)
    {
        _options = options;
        _colorManager = new AnsiColorManager();
    }
    
    public void WriteCharacterAt(int x, int y, Rune character, CharacterAttribute attribute)
    {
        WriteCharactersAt(x, y, 1, character, attribute);
    }

    public void WriteCharactersAt(int x, int y, int count, Rune character, CharacterAttribute attribute)
    {
        var sb = new StringBuilder();
        sb.Append(_colorManager.GetCursorSequence(x, y));
        sb.Append(_colorManager.GetAttributeSequence(attribute));

        Span<char> buf = stackalloc char[2];                // Assume a rune is at most 2 UTF16 codeunits length
        for (var idx = 0; idx < count; idx++)
        {
            if (character.Utf16SequenceLength > 1)
            {
                character.EncodeToUtf16(buf);
                sb.Append(buf);
            }
            else
            {
                sb.Append((char)character.Value);
            }
        }

        System.Console.Write(sb.ToString());
    }

    public void SetCursorAt(int x, int y)
    {
        throw new NotImplementedException();
    }
    
    public void SetCursorVisibility(bool isVisible)
    {
        if (isVisible)
        {
            System.Console.Write(AnsiEscapeSequences.DECTCEM_VISIBLE);
        }
        else
        {
            System.Console.Write(AnsiEscapeSequences.DECTCEM_HIDDEN);
        }
    }
    
    public void ClearScreen(TvColor fore, TvColor back)
    {
        System.Console.Write(AnsiEscapeSequences.CLEAR);
    }

    public void SetWindowsSize(TvBounds value)
    {
        throw new NotImplementedException();
    }

    public bool CanChangeWindowsSize { get; }
}