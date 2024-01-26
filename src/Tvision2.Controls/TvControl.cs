using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public abstract class TvControl
{
    internal const string CONTROL_TAG = "Tvision2::Control";
    public Guid Id { get; } = Guid.NewGuid();
    protected readonly TvComponent _component;

    public TvComponent AsComponent() => _component;

    public string Name { get; set; } = "";

    public TvControl(TvComponent component)
    {
        _component = component;
        _component.Metadata.TagWith(CONTROL_TAG, new object());
    }
}

public class TvControl<TState, TOptions> : TvControl
{
    private readonly TOptions _options;
    
    internal TvControl(TvComponent<TState> cmp, TOptions options) : base(cmp)
    {
        _options = options;
    }
}