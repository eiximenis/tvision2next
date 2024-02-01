using Microsoft.Extensions.DependencyInjection;
using Tvision2.Console;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Engine;

public interface ITvision2Engine
{
     T GetRegisteredComponent<T>() where T: notnull;
     TvUiManager UI { get; }
}

public class Tvision2Engine : ITvision2Engine
{
    public bool Running { get; private set; } = false;
    private readonly Tvision2Options _options;
    private readonly IConsoleDriver _consoleDriver;
    private readonly TvConsoleEvents _consoleEvents;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConsoleEventsReader _eventsReader;
    public TvUiManager UI { get; }
    
    public Tvision2Engine(Tvision2Options options, IServiceProvider sp)
    {
        _options = options;
        _serviceProvider = sp;
        _consoleDriver = new AnsiConsoleDriver(_options.ConsoleOptions);
        _consoleEvents = new TvConsoleEvents();
        var crows = System.Console.WindowHeight;
        var ccols = System.Console.WindowWidth;
        UI = new TvUiManager(new VirtualConsole(TvBounds.FromRowsAndCols(crows,ccols), TvColor.Black), _consoleDriver);
        UI.ComponentTree.Add(_options.BackgroundDefinition.CreateBackgroundComponent(), LayerSelector.Background);
        _eventsReader = IConsoleEventsReader.GetByOs();
    }
    
    public async Task Initialize()
    {
        Running = true;
        ResolveHooks();
        await InvokeStartup();
    }

    private async Task InvokeStartup()
    {
        
    }

    private void ResolveHooks()
    {
        var hooks = _serviceProvider.GetServices<IHook>();
        foreach (var hook in hooks)
        {
            UI.AddHook(hook);
        }
    }

    internal async Task NextCycle()
    {
        _consoleEvents.Clear();
        _eventsReader.ReadEvents(_consoleEvents);
        await UI.BeforeUpdate(_consoleEvents);
        await UI.Update(_consoleEvents);
        await UI.CalculateLayout();
        UI.Draw();
    }

    internal async Task Teardown()
    {
    }

    public T GetRegisteredComponent<T>() where T: notnull => _serviceProvider.GetRequiredService<T>();
}