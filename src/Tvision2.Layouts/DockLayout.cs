using System.Diagnostics;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts;

public enum Dock
{
    Fill = 0,
    Top = 1,
    Left = 2,
    Bottom = 3,
    Right = 4
}


public enum HorizontalAlignment
{
    None = 0,
    Left,       // Viewport starts at same column as parent (+ Distance)
    Right,      // Viewport ends at same column as parent (- Distance)      
    Stretch     // Viewport has same width as parent (- Distance)
}

public enum VerticalAlignment
{
    None = 0,       
    Top,        // Viewport starts at same row at parent (+ Distance)
    Bottom,     // Viewport ends at same row at parent (- Distance)
    Stretch     // Viewport has same height as parent (- Distance)
}

class DockLayout : ILayoutManager
{
    private readonly ITvContainer _container;
    private readonly HorizontalAlignment _horizontalAlignment;
    private readonly VerticalAlignment _verticalAlignment;

    public bool HasLayoutPending { get; private set; }

    public DockLayout(ITvContainer container, Dock dock)
    {
        _container = container;
        _container.On().ViewportUpdated.Do(OnContainerUpdated);

        (_horizontalAlignment, _verticalAlignment) = dock switch
        {
            Dock.Left => (HorizontalAlignment.Left, VerticalAlignment.Stretch),
            Dock.Right => (HorizontalAlignment.Right, VerticalAlignment.Stretch),
            Dock.Top => (HorizontalAlignment.Stretch, VerticalAlignment.Top),
            Dock.Bottom => (HorizontalAlignment.Stretch, VerticalAlignment.Bottom),
            _ => (HorizontalAlignment.Stretch, VerticalAlignment.Stretch), // Dock.Fill
        };
    }

    private void OnContainerUpdated(ViewportUpdateReason reason)
    {
        HasLayoutPending = true;
    }

    public ViewportUpdateReason UpdateLayout(Viewport viewportToUpdate)
    {
        var (pos, bounds) = RecalculateBoundsAndPosition(viewportToUpdate);
        var updateReason = viewportToUpdate.MoveTo(pos);
        updateReason |= viewportToUpdate.Resize(bounds);
        
        HasLayoutPending = false;
        return updateReason;
    }

    private (TvPoint Position, TvBounds Bounds) RecalculateBoundsAndPosition(Viewport viewportToUpdate)
    {
        var relative = _container.Viewport;
        var desiredBounds = viewportToUpdate.Bounds;
        var desiredPosition = viewportToUpdate.Position;
        if (relative.IsNull)
        {
            return (viewportToUpdate.Position, viewportToUpdate.Bounds);
        }

        var parentTopLeft = relative.Position; // + TvPoint.FromXY(Distance.Left, Distance.Top);
        var parentBottomRight = relative.BottomRight; //- TvPoint.FromXY(Distance.Right, Distance.Bottom);
        var relativePos = desiredPosition + parentTopLeft;
        // Position is relative to the relative
        var (childStartCol, childStartRow) = relativePos;
        var (childEndCol, childEndRow) = relativePos + desiredBounds;

        switch (_horizontalAlignment)
        {
            case HorizontalAlignment.Left:
                childStartCol = parentTopLeft.X;
                childEndCol = desiredBounds.Width == 0 ? childStartCol : childStartCol + desiredBounds.Width - 1;
                break;
            case HorizontalAlignment.Right:
                childEndCol = parentBottomRight.X;
                childStartCol = desiredBounds.Width == 0 ? childEndCol : childEndCol - desiredBounds.Width + 1;
                break;
            case HorizontalAlignment.Stretch:
                childEndCol = parentBottomRight.X;
                childStartCol = parentTopLeft.X;
                break;
            default: break;
        }

        switch (_verticalAlignment)
        {
            case VerticalAlignment.Top:
                childStartRow = parentTopLeft.Y;
                childEndRow = desiredBounds.Height == 0 ? childStartRow : childStartRow + desiredBounds.Height - 1;
                break;
            case VerticalAlignment.Bottom:
                childEndRow = parentBottomRight.Y; //- _distance.Bottom;
                childStartRow = desiredBounds.Height == 0 ? childEndRow : childEndRow - desiredBounds.Height + 1;
                break;
            case VerticalAlignment.Stretch:
                childStartRow = parentTopLeft.Y;
                childEndRow = parentBottomRight.Y;
                break;
            default: break;
        }

        var position = TvPoint.FromXY(childStartCol, childStartRow);
        var bounds = TvPoint.CalculateBounds(position, TvPoint.FromXY(childEndCol, childEndRow));
        return (position, bounds);
    }
}