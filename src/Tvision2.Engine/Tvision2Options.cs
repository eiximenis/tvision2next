using System.Reflection.Metadata;
using Tvision2.Console;
using Tvision2.Core;
using Tvision2.Engine.Components.Backgrounds;

namespace Tvision2.Engine;


public interface ITvision2Options
{
    ITvision2Options AddConsoleOptions(Action<IConsoleOptions>? optionsAction = null);
    ITvision2Options WithBackground(BackgroundDefinition background);
    ITvision2Options WithBackgroundColor(TvColor color);
    ITvision2Options AddHook<T>() where T : IHook;

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

    ITvision2Options ITvision2Options.WithBackgroundColor(TvColor color)
    {
        _backgroundDefinition = DefaultBackgroundDefinitionsProvider.SolidColorBackground(color);
        return this;
    }

    ITvision2Options ITvision2Options.AddHook<T>()
    {
        return this;
    }

}