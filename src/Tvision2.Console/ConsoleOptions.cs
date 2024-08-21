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