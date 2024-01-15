namespace Tvision2.Console;

public interface IConsoleOptions
{
    IConsoleOptions UseAlternateBuffer();
}

public class ConsoleOptions : IConsoleOptions
{
    public bool UseAlternateBuffer { get; private set; }
    IConsoleOptions IConsoleOptions.UseAlternateBuffer()
    {
        UseAlternateBuffer = true;
        return this;
    }
    
}