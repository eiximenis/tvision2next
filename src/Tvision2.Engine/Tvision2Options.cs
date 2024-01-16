using Tvision2.Console;
using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Tvision2.Engine.Components.Backgrounds;

namespace Tvision2.Core.Engine;


public interface ITvision2Options
{
    ITvision2Options AddConsoleOptions(Action<IConsoleOptions>? optionsAction = null);
    ITvision2Options WithBackground(BackgroundDefinition background);
}
public class Tvision2Options : ITvision2Options
{
    public ConsoleOptions ConsoleOptions { get; } = new ();
    private BackgroundDefinition? _backgroundDefinition;

    public BackgroundDefinition BackgroundDefinition => _backgroundDefinition ?? DefaultBackgroundDefinitionsProvider.SolidColorBackground(TvColor.Black);

    ITvision2Options ITvision2Options.AddConsoleOptions(Action<IConsoleOptions>? optionsAction)
    {
        optionsAction?.Invoke(ConsoleOptions);
        return this;
    }

    ITvision2Options ITvision2Options.WithBackground(BackgroundDefinition definition)
    {
        _backgroundDefinition = definition;
        return this;
    }
}