using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine.Layouts;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Components;

public abstract class TvComponent
{
    public TvComponentMetadata Metadata {get; }

    public Guid Id { get; }
    private Viewport _viewport;
    public LayerSelector Layer { get; private set; }

    private bool _invalidated;
    private bool _stateChanged;
    
    public ILayoutManager Layout { get; private set; }

    protected TvComponent(Viewport? viewport, LayerSelector layerSelector)
    {
        _invalidated = false;
        _stateChanged = false;
        Id = Guid.NewGuid();
        Metadata = new TvComponentMetadata(this);
        _viewport = viewport ?? Viewports.Null();
        // _viewport.OnUpdated += Viewport_Updated;
        Layer = layerSelector;
        Layout = LayoutManagers.Absolute;
    }

    public void Invalidate() => _invalidated = true;

    protected void StateChanged() => _stateChanged = true;

    internal void MoveToLayer(LayerSelector layer) => Layer = layer;

    public void UseLayout(ILayoutManager layout)
    {
        Layout.Dismiss();
        Layout = layout;
    }

    private void Viewport_Updated(object? _, ViewportUpdateReason reason)
    {
        if (reason == ViewportUpdateReason.Resized)
        {
            UpdateAdaptativeDrawersForUpdatedViewport();
        }
    }
    public Viewport Viewport => _viewport;
    
    public abstract void Draw(VirtualConsole console);

    public DirtyStatus Update(UpdateContext context)
    {
        var status = DoUpdate(context);
        if (_invalidated)
        {
            _invalidated = false;
            status |= DirtyStatus.Invalidated;
        }

        if (_stateChanged)
        {
            _stateChanged = false;
            status |= DirtyStatus.StateChanged;
        }

        return status;
    }
    public abstract DirtyStatus DoUpdate(UpdateContext ctx);
    
    

    protected abstract void UpdateAdaptativeDrawersForUpdatedViewport();

    /// <summary>
    /// Easy shortcut for creating stateless components
    /// </summary>
    public static TvComponent<Unit> CreateStatelessComponent(Viewport? viewport = null) => new TvComponent<Unit>(Unit.Value, viewport);
    public static TvComponent<Unit> CreateStatelessComponent(LayerSelector layerSelector, Viewport? viewport = null) => new TvComponent<Unit>(Unit.Value, viewport, layerSelector);

    public static TvComponent<T> Create<T>(T state, Viewport? viewport = null) => new TvComponent<T>(state, viewport);
    
}

public sealed class TvComponent<T> : TvComponent
{
    class AdaptativeDrawerDefinition<TC>
    {
        public Func<Viewport, bool> Selector { get; }
        public List<ITvDrawer<TC>> Drawers { get; }

        public AdaptativeDrawerDefinition(Func<Viewport, bool> selector)
        {
            Selector = selector;
            Drawers = new List<ITvDrawer<TC>>();
        }
    }

    private readonly List<AdaptativeDrawerDefinition<T>> _adaptativeDrawerDefinitions;
    private readonly List<ITvDrawer<T>> _adaptativeDrawersSelected;

    private readonly List<ITvDrawer<T>> _drawers;
    private readonly List<ITvBehavior<T>> _behaviors;

    private readonly List<Func<IServiceProvider, ITvBehavior<T>>> _behaviorCreators;

    private readonly BehaviorContext<T> _behaviorContext;
    public T State { get; private set; }


    public TvComponent(T initialState, Viewport? viewport) : this(initialState, viewport, LayerSelector.Standard)
    {
    }

    public TvComponent(T initialState, Viewport? viewport, LayerSelector layerSelector) : base(viewport, layerSelector)
    {
        State = initialState;
        _drawers = new List<ITvDrawer<T>>();
        _behaviors = new List<ITvBehavior<T>>();
        _behaviorCreators = new List<Func<IServiceProvider, ITvBehavior<T>>>();
        _adaptativeDrawerDefinitions = new List<AdaptativeDrawerDefinition<T>>();
        _adaptativeDrawersSelected = new List<ITvDrawer<T>>();
        _behaviorContext = new BehaviorContext<T>(this);
    }

    public void SetState(T newState)
    {
        State = newState;
        StateChanged();
    }

    public void AddDrawer(ITvDrawer<T> drawer) => _drawers.Add(drawer);

    public void AddDrawer(Action<ConsoleContext> drawerAction) => AddDrawer(new StatelessFuncDrawer<T>(drawerAction));
    public void AddDrawer(Action<ConsoleContext, T> drawerAction) => AddDrawer(new FuncDrawer<T>(drawerAction));

    internal void AddAdaptativeDrawerDefinition(ITvDrawer<T> drawer, Func<Viewport, bool> condition)
    {
        var entry = _adaptativeDrawerDefinitions.FirstOrDefault(add => add.Selector == condition);
        if (entry is null)
        {
            entry = new AdaptativeDrawerDefinition<T>(condition);
            _adaptativeDrawerDefinitions.Add(entry);
        }

        entry.Drawers.Add(drawer);
    }

    public void AddBehavior(Func<T, T> newStateGenerator) =>
        AddBehavior(newStateGenerator, StateEqualityComparer.ByReference);

    public void AddBehavior(Func<T, T> newStateGenerator, StateEqualityComparer comparer)
    {
        _behaviors.Add(new NewStateBehavior<T>(newStateGenerator, comparer));
    }

    public void AddBehavior(Action<BehaviorContext<T>> behaviorAction)
    {
        _behaviors.Add(new ActionBehavior<T>(behaviorAction));
    }


    public void AddBehavior(ITvBehavior<T> behavior)
    {
        _behaviors.Add(behavior);
    }

    public override DirtyStatus DoUpdate(UpdateContext ctx)
    {
        var events = ctx.ConsoleEvents;
        _behaviorContext.SetData(events, ctx.LastElapsed);
        foreach (var behavior in _behaviors)
        {
            behavior.Do(_behaviorContext);
        }
        var result = _behaviorContext.ApplyChanges();
        return result;
    }
    
    public override void Draw(VirtualConsole console)
    {
        if (Viewport.IsNull) return;
        
        var context = new ConsoleContext(console, Viewport);
        foreach (var drawer in _adaptativeDrawersSelected)
        {
            drawer.Draw(context, State);
        }

        foreach (var drawer in _drawers)
        {
            var drawResult = drawer.Draw(context, State);
            if (drawResult != DrawResult.Done)
            {
                context = context.WithDrawResultsApplied(drawResult);
            }
        }
    }
    
    protected override void UpdateAdaptativeDrawersForUpdatedViewport()
    {
        _adaptativeDrawersSelected.Clear();
        foreach (var definition in _adaptativeDrawerDefinitions)
        {
            if (definition.Selector(Viewport))
            {
                _adaptativeDrawersSelected.AddRange(definition.Drawers);
            }
        }
    }
}