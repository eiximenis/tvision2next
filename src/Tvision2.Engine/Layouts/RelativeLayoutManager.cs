using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Engine.Layouts
{

    class MarginViewportSnapshotTransformer : IViewportSnapshotTransformer
    {

        private readonly Margin _margin;
        public MarginViewportSnapshotTransformer(Margin margin)
        {
            _margin = margin;
        }
        public IViewportSnapshot Transform(IViewportSnapshot snapshot)
        {
            var pos = snapshot.Position;
            var newPos = TvPoint.FromXY(pos.X + _margin.Left, pos.Y + _margin.Top);
            var bounds = snapshot.Bounds;
            var newBounds = TvBounds.FromRowsAndCols(bounds.Height - (_margin.Top + _margin.Bottom), bounds.Width - (_margin.Left + _margin.Right));
            return new ViewportSnapshot(newPos, newBounds);
        }
    }

    public interface IViewportSnapshotTransformer
    {
        IViewportSnapshot Transform(IViewportSnapshot snapshot);
    }

    class RelativeLayoutManager : ILayoutManager
    {
        private ITvContainer _owner;
        public bool HasLayoutPending { get; private set; }
        private readonly IViewportSnapshotTransformer? _transformer;
        public RelativeLayoutManager(ITvContainer owner, IViewportSnapshotTransformer? transformer)
        {
            _owner = owner;
            _owner.On().ViewportUpdated.Do(OnContainerUpdated);
            _transformer = transformer;
        }

        private void OnContainerUpdated(ViewportUpdateReason reason)
        {
            HasLayoutPending = true;
        }

        public ViewportUpdateReason UpdateLayout(Viewport viewportToUpdate)
        {
            var vpSnapshot = _transformer is not null ? _transformer.Transform(_owner.Viewport) : _owner.Viewport;
            var pos = vpSnapshot.Position;
            var bounds = vpSnapshot.Bounds;
            var updateReason = viewportToUpdate.MoveAndResize(pos, bounds);
            HasLayoutPending = false;
            return updateReason;
        }
    }

}
