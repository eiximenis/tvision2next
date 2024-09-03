using System.Collections.ObjectModel;
using System.Diagnostics;
using Tvision2.Console.Colors;
using Tvision2.Core;

namespace Tvision2.Console;

public static partial  class TvConsole
{
    private static readonly AnsiColorManager _colorManager = new();
    private static TvColor _foreground;
    private static bool _writeForeground = false;
    private static TvColor _background;
    private static bool _writeBackground= false;
    internal static TvConsoleDrawer ConsoleDrawer { get; } = new();

    public static TvColor Foreground
    {
        get => _foreground;
        set
        {
            if (value != _foreground)
            {
                _foreground = value;
                _writeForeground = true;
            }
        }
    }
    public static TvColor Background
    {
        get => _background;
        set
        {
            if (value != _background)
            {
                _background = value;
                _writeBackground = true;
            }
        }
    }
    
    
    public static void Write(string msg)
    {
        UpdateTerminalColors();
        System.Console.Write(msg);
    }

    public static void Write(char character)
    {
        UpdateTerminalColors(); 
        System.Console.Write(character);
    }
    public static void Write(string msg, int top, int left)
    {
        MoveCursorTo(left, top);
        Write(msg);
    }

    public static void Write(char character, int top, int left)
    {
        MoveCursorTo(left, top);
        Write(character);
    }

    public static void WriteLine(string msg)
    {
        UpdateTerminalColors();
        System.Console.WriteLine(msg);
    }

    private static void UpdateTerminalColors()
    {
        if (_writeForeground)
        {
            System.Console.Write(_colorManager.GetForegroundAttributeSequence(_foreground));
            _writeForeground = false;
        }
        if (_writeBackground)
        {
            System.Console.Write(_colorManager.GetBackgroundAttributeSequence(_background));
            _writeBackground = false;
        }
    }


    public static void MoveCursorTo(int x, int y)
    {
        var seq = _colorManager.GetCursorSequence(x, y);
        System.Console.Write(seq);
    }
}

