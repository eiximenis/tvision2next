using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public abstract class TvControl
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public string Name { get; }

    public TvControl(string name)
    {
        Name = name;
    }
}

public class TvControl<TState, TOptions> : TvControl
{
    private readonly TvComponent<TState> _component;
    
    public TvControl(string name, TState state) : base(name)
    {
        _component = TvComponent.Create(state);
    }

    public TvComponent AsComponent()
    {
        return _component;
    }
}