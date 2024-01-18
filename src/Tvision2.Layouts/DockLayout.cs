using System.Diagnostics;
using Tvision2.Engine.Components;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts;

public enum Dock
{
    Fill = 0,
    Top = 1,
    Left = 1,
    Bottom = 2
}

class DockLayout : ILayoutManager
{
    private readonly TvComponentMetadata _container;

    public bool HasLayoutPending { get; private set; } 

    public DockLayout(TvComponentMetadata container)
    {
        _container = container;
        _container.ViewportUpdated.Do(OnContainerUpdated);
    }

    private void OnContainerUpdated(ViewportUpdateReason obj)
    {
        HasLayoutPending = true;
    }

    public ViewportUpdateReason UpdateLayout(TvComponentMetadata metadata)
    {
        Debug.WriteLine("Recalculating Layout.");
        HasLayoutPending = false;
        return ViewportUpdateReason.None;
    }
}