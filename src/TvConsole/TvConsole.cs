using System.Collections.ObjectModel;
using Tvision2.Console.Colors;
using Tvision2.Core;

namespace Tvision2.Console;

public static partial  class TvConsole
{    
    
    private static readonly  AnsiColorManager _colorManager;
    private static TvColor _foreground;
    private static bool _writeForeground = false;
    private static TvColor _background;
    private static bool _writeBackground= false;

    static TvConsole()
    {
        _colorManager = new AnsiColorManager();
    }

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
        System.Console.Write(msg);
    }
    
    public static void WriteLine(string msg)
    
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
        System.Console.WriteLine(msg);
    }
}

