using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts
{
    class CellGridContainer : ITvContainer, ITvContainerActions
    {
        private readonly GridContainer _grid;
        private readonly CellRef _cell;
        private readonly ActionsChain<ViewportUpdateReason> _viewportUpdated = new();
        private readonly Viewport _viewport = Viewports.Null();

        public bool HasLayoutPending { get; private set; }

        public CellGridContainer(GridContainer grid, CellRef cell)
        {
            _grid = grid;
            _cell = cell;
        }

        internal async Task GridViewportUpdated()
        {
            var (pos, bounds) = RecalculateBoundsAndPosition();
            var updateReason = _viewport.MoveTo(pos);
            updateReason |= _viewport.Resize(bounds);

            if (updateReason.Any())
            {
                await _viewportUpdated.Invoke(updateReason);
            }
        }

        private (TvPoint Pos, TvBounds Bounds) RecalculateBoundsAndPosition()
        {
            var gridViewport = _grid.Viewport;
            var cellWidth = gridViewport.Bounds.Width / _grid.Columns;
            var cellHeight = gridViewport.Bounds.Height / _grid.Rows;
            var pos = TvPoint.FromXY(gridViewport.Position.X + _cell.Column * cellWidth, gridViewport.Position.Y + _cell.Row * cellHeight);
            var bounds = TvBounds.FromRowsAndCols(cellHeight, cellWidth);
            return (pos, bounds);
        }

        IViewportSnapshot ITvContainer.Viewport => _viewport;
        ITvContainerActions ITvContainer.On() => this;
        IActionsChain<ViewportUpdateReason> ITvContainerActions.ViewportUpdated => _viewportUpdated;
    }                                                                                                                                                                       
}
 