﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    private readonly GridDefinition _gridDefinition;
    private readonly ITvContainer _owner;
    

    internal IViewportSnapshot Viewport => _owner.Viewport;

    public int Rows => _gridDefinition.Rows;
    public int Columns => _gridDefinition.Columns;

    public GridContainer(ITvContainer owner, GridDefinition definition)
    {
        _owner = owner;
        owner.On().ViewportUpdated.Do(OnContainerUpdated);
        _gridDefinition = definition;
        for (var row = 0; row < _gridDefinition.Rows; row++)
        {
            for (var col = 0; col < _gridDefinition.Columns; col++)
            {
                var cellRef = new CellRef(row, col);
                _cellsLayouts[cellRef] = new CellGridContainer(this, cellRef);
            }
        }
    }

    private async Task OnContainerUpdated(ViewportUpdateReason obj)
    {
        for (var row = 0; row < _gridDefinition.Rows; row++)
        {
            for (var col = 0; col < _gridDefinition.Columns; col++)
            {
                var cellRef = new CellRef(row, col);
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

public class GridDefinition
{
    public int Rows { get; } = 2;
    public int Columns { get; } = 2;

}
