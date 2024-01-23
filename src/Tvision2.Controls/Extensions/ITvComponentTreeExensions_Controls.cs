using Tvision2.Engine.Components;

namespace Tvision2.Controls.Extensions;

public static class ITvComponentTreeExensions_Controls
{

    public static Task<TvComponentTreeNode> Add<TState, TOptions>(this ITvComponentTree tree,
        TvControl<TState, TOptions> control) => tree.Add(control.AsComponent());

    public static Task<TvComponentTreeNode> AddChild<TState, TOptions>(this ITvComponentTree tree,
        TvControl<TState, TOptions> child, TvControl parent) => tree.AddChild(child.AsComponent(), parent.);

}