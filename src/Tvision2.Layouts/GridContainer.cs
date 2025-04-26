using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Drawing.Tables;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts;

readonly record struct CellRef(int Row, int Column);

/// <summary>
/// This class allows to create grid layouts. It provides a container for each cell in the grid.
/// Components can attach a layout to each cell container to define how the component will be displayed in the cell.
/// </summary>
public class GridContainer
{

    private readonly Dictionary<CellRef, CellGridContainer> _cellsLayouts = [];
    private readonly TableDefinition _gridDefinition;
    private readonly ITvContainer _owner;

    internal TableDefinition Definition => _gridDefinition;
    
    internal IViewportSnapshot Viewport => _owner.Viewport;
    public int Rows => _gridDefinition.RowsCount;
    public GridContainer(ITvContainer owner, TableDefinition definition)
    {
        _owner = owner;
        owner.On().ViewportUpdated.Do(OnContainerUpdated);
        _gridDefinition = definition;
        for (var rowIdx = 0; rowIdx < _gridDefinition.RowsCount; rowIdx++)
        {
            var row = _gridDefinition.Rows[rowIdx];
            for (var col = 0; col < row.CellsCount ; col++)
            {
                var cellRef = new CellRef(rowIdx, col);
                _cellsLayouts[cellRef] = new CellGridContainer(this, cellRef);
            }
        }

        OnContainerUpdated(ViewportUpdateReason.MovedAndResized).Wait();
    }

    private async Task OnContainerUpdated(ViewportUpdateReason obj)
    {

        TableDrawer.CalculateCellsBounds(_gridDefinition, Viewport.Bounds);

        for (var rowIdx = 0; rowIdx < _gridDefinition.RowsCount; rowIdx++)
        {
            var row = _gridDefinition.Rows[rowIdx];
            for (var col = 0; col < row.CellsCount; col++)
            {
                var cellRef = new CellRef(rowIdx, col);
                await _cellsLayouts[cellRef].GridViewportUpdated();
            }
        }
    }

    public ITvContainer At(int row, int column)
    {
        var cellRef = new CellRef(row, column);
        return _cellsLayouts[cellRef];
    }

}
