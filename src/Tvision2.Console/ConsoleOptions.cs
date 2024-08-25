using Tvision2.Console.Windows;

namespace Tvision2.Console;

public interface IConsoleOptions
{
    IConsoleOptions UseAlternateBuffer();
    IConsoleOptions ShowCursor();
}

public class ConsoleOptions : IConsoleOptions
{
    public bool UseAlternateBuffer { get; private set; } = false;

    public CursorVisibility CursorVisibility { get; private set; }  = CursorVisibility.Hidden;
    public WindowsConsoleOptions Windows { get; }

    public ConsoleOptions()
    {
        Windows = new WindowsConsoleOptions();
    }

    

    IConsoleOptions IConsoleOptions.ShowCursor()
    {
        CursorVisibility = CursorVisibility.Visible;
        return this;
    }

    IConsoleOptions IConsoleOptions.UseAlternateBuffer()
    {
        UseAlternateBuffer = true;
        return this;
    }
    
}

public enum CursorVisibility
{
    Hidden,
    Visible
}