using System.Diagnostics;
using Tvision2.Console.Events;
using Tvision2.Engine;

namespace Tvision2.Controls;

public class TvControlsTabKeyHook : IHook
{
    private readonly TvControlsOptions _options;
    public TvControlsTabKeyHook(TvControlsOptions options, TvControlsTree controlsTree)
    {
        _options = options;
    }

    public async Task BeforeUpdate(TvConsoleEvents events)
    {
        
    }
}

public class TvControlsOptions : ITvControlsOptions
{
}

public interface ITvControlsOptions
{
}