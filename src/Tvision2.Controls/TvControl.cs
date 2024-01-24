using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public abstract class TvControl
{
    public Guid Id { get; } = Guid.NewGuid();
    protected readonly TvComponent _component;

    public TvComponent AsComponent() => _component;

    public string Name { get; set; } = "";

    public TvControl(TvComponent component)
    {
        _component = component;
    }
}

public class TvControl<TState, TOptions> : TvControl
{
    private readonly TOptions _options;

    public TvControl(TState state, TOptions options) : base(TvComponent.Create(state))
    {
        _options = options;
    }
}