using System;
using System.Collections.Generic;
using System.Data;
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
            var row = _grid.Definition.Rows[_cell.Row];
            var cell = row.CellAt(_cell.Column);
            var cellWidth = cell.ComputedWidth;
            var cellHeight = row.ComputedHeight;

            var accHeight = 0;
            for (var prevRow = 0; prevRow < _cell.Row; prevRow++)
            {
                accHeight += _grid.Definition.Rows[prevRow].ComputedHeight;
            }
            var accWidth = 0;
            for (var prevCol = 0; prevCol < _cell.Column; prevCol++)
            {
                accWidth += _grid.Definition.Rows[_cell.Row].CellAt(prevCol).ComputedWidth;
            }

            var pos = TvPoint.FromXY(gridViewport.Position.X + accWidth , gridViewport.Position.Y + accHeight);
            var bounds = TvBounds.FromRowsAndCols(cellHeight, cellWidth);
            return (pos, bounds);
        }

        IViewportSnapshot ITvContainer.Viewport => _viewport;
        ITvContainerActions ITvContainer.On() => this;
        IActionsChain<ViewportUpdateReason> ITvContainerActions.ViewportUpdated => _viewportUpdated;
    }                                                                                                                                                                       
}
 