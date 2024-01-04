using Tvision2.Core.Console;

namespace Tvision2.Core.Engine;


public interface ITvision2Options
{
    ITvision2Options AddConsoleOptions(Action<IConsoleOptions>? optionsAction = null);
}
public class Tvision2Options : ITvision2Options
{
    public ConsoleOptions ConsoleOptions { get; } = new ConsoleOptions(); 
    
    ITvision2Options ITvision2Options.AddConsoleOptions(Action<IConsoleOptions>? optionsAction = null)
    {
        optionsAction?.Invoke(ConsoleOptions);
        return this;
    }
}