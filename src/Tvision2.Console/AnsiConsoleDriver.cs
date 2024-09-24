using System.Text;
using Tvision2.Console.Colors;
using Tvision2.Core;
using Tvision2.Core.Console;

namespace Tvision2.Console;

public class AnsiConsoleDriver : IConsoleDriver
{
    private readonly AnsiColorManager _colorManager;
    private readonly ConsoleOptions _options;

    public AnsiConsoleDriver(ConsoleOptions options)
    {
        _options = options;
        _colorManager = new AnsiColorManager();
    }

    public void Init()
    {
        System.Console.OutputEncoding = System.Text.Encoding.Unicode;

        if (_options.UseAlternateBuffer)
        {
            System.Console.Write(AnsiEscapeSequences.SMCUP);
        }

        if (_options.CursorVisibility == CursorVisibility.Hidden)
        {
            SetCursorVisibility(false);
        }
    }

    public void Teardown()
    {
        if (_options.UseAlternateBuffer)
        {
            System.Console.Write(AnsiEscapeSequences.RMCUP);
        }

        if (_options.CursorVisibility == CursorVisibility.Hidden)
        {
            SetCursorVisibility(true);
        }
    }


    public void WriteCharacter(Rune character, int count = 1)
    {
        if (character.Utf16SequenceLength == 1)
        {
            for (var idx = 0; idx < count; idx++)
            {
                System.Console.Write((char)character.Value);
            }
        }
        else
        {
            Span<char> buf = stackalloc char[2];
            character.EncodeToUtf16(buf);
            for (var idx = 0; idx < count; idx++)
            {
                System.Console.Out.Write(buf);
            }
        }


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
        WriteCharacter(character, count);
    }

    public void SetCursorAt(int x, int y)
    {
        System.Console.Write(_colorManager.GetCursorSequence(x, y));
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