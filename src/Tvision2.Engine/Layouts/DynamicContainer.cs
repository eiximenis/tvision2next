using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;

namespace Tvision2.Engine.Layouts
{
    /// <summary>
    /// This class is a container that can be moved and/or resized manually
    /// </summary>
    public class DynamicContainer : ITvContainer, ITvContainerActions
    {
        private readonly ActionsChain<ViewportUpdateReason> _viewportUpdated = new();
        private ViewportSnapshot _viewport;
        public IActionsChain<ViewportUpdateReason> ViewportUpdated => _viewportUpdated;
        public IViewportSnapshot Viewport => _viewport;

        public DynamicContainer(IViewportSnapshot viewport)
        {
            _viewport = new ViewportSnapshot(viewport.Position, viewport.Bounds);
        }

        public DynamicContainer(ViewportSnapshot viewport)
        {
            _viewport = new ViewportSnapshot(viewport.Position, viewport.Bounds);
        }

        public DynamicContainer(TvPoint pos, TvBounds bounds)
        {
            _viewport = new ViewportSnapshot(pos, bounds);
        }

        public ViewportUpdateReason MoveTo(TvPoint pos)
        {
            if (pos != _viewport.Position)
            {
                _viewport = _viewport with { Position = pos };
                return ViewportUpdateReason.Moved;
            }
            return ViewportUpdateReason.None;
        }
        public ViewportUpdateReason Resize(TvBounds bounds)
        {
            if (bounds != _viewport.Bounds)
            {
                _viewport = _viewport with { Bounds = bounds };
                return ViewportUpdateReason.Resized;
            }
            return ViewportUpdateReason.None;
        }

        public ViewportUpdateReason MoveAndResize(TvPoint pos, TvBounds bounds)
        {
            var updateReason = MoveTo(pos);
            updateReason |= Resize(bounds);
            return updateReason;
        }

        public ViewportUpdateReason ChangeViewport<TV>(TV snapshot) where TV: IViewportSnapshot
        {
            var updateReason = MoveTo(snapshot.Position);
            updateReason |= Resize(snapshot.Bounds);
            return updateReason;
        }

        public ITvContainerActions On() => this;
    }
}
