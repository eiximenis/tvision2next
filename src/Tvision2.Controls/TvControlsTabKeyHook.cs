using System.Diagnostics;
using Tvision2.Console.Events;
using Tvision2.Engine;

namespace Tvision2.Controls;

public class TvControlsTabKeyHook : IHook
{
    private readonly TvControlsOptions _options;
    private readonly TvControlsTree _controlsTree;  
    public TvControlsTabKeyHook(TvControlsOptions options, TvControlsTree controlsTree)
    {
        _options = options;
        _controlsTree = controlsTree;
    }

    public async Task BeforeUpdate(TvConsoleEvents events)
    {
        var tabEvent = events.AcquireFirstKeyboard(evt => evt.AsConsoleKeyInfo().Key == ConsoleKey.Tab, autoHandle: true);
        if (tabEvent is not null)
        {
            _controlsTree.FocusNext();
        }
    }
}

public class TvControlsOptions : ITvControlsOptions
{
}

public interface ITvControlsOptions
{
}