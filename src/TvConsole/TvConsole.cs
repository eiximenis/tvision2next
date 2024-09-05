using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Tvision2.Console.Colors;
using Tvision2.Core;

namespace Tvision2.Console;

public static partial class TvConsole
{
    private static readonly ConsoleOptions _options = new();
    private static readonly Lazy<AnsiConsoleDriver> _consoleDriver = new(() =>
    {
        var driver = new AnsiConsoleDriver(_options);
        driver.Init();
        return driver;
    });

    private static AnsiConsoleDriver ConsoleDriver => _consoleDriver.Value;

    private static readonly AnsiColorManager _colorManager = new();
    private static TvColor _foreground;
    private static bool _writeForeground = false;
    private static TvColor _background;
    private static bool _writeBackground = false;
    internal static TvConsoleDrawer ConsoleDrawer { get; } = new();


    public static void MoveCursorTo(int left, int top) => ConsoleDriver.SetCursorAt(left, top);

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


    public static void Write(string msg, int top, int left)
    {
        ConsoleDriver.SetCursorAt(left, top);
        Write(msg);
    }

    public static void Write(char character)
    {
        UpdateTerminalColors();
        Write(new Rune(character));
    }

    public static void Write(Rune rune, int top, int left)
    {
        ConsoleDriver.SetCursorAt(left, top);
        Write(rune);
    }
    public static void Write(Rune rune)
    {
        UpdateTerminalColors();
        ConsoleDriver.WriteCharacter(rune, 1);
    }

    public static void Write(char character, int top, int left) 
    {
        ConsoleDriver.SetCursorAt(left, top);
        Write(new Rune(character));
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
}

