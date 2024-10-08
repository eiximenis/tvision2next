﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Drawing.Borders;

namespace Tvision2.Drawing.Tables;


public interface IBoundedElement
{
    TvBounds CalculateDesiredBounds();
    void Draw<TD>(TvBounds bounds, TD drawer) where TD: IConsoleDrawer;
}

public class StringBoundedElement : IBoundedElement
{
    private readonly string _value;
    private readonly int _len;

    public StringBoundedElement(string value)
    {
        _value = value ?? "";
        _len = Length.WidthFromString(_value);
    } 

    public TvBounds CalculateDesiredBounds()
    {
        return TvBounds.FromRowsAndCols(1, _len);
    }

    public void Draw<TD>(TvBounds bounds, TD consoleDrawer) where TD: IConsoleDrawer
    {
        consoleDrawer.DrawStringAt(_value, TvPoint.Zero, TvColorsPair.FromForegroundAndBackground(TvColor.White, TvColor.Black));
    }
}

public class Table
{
    private readonly BorderValue _border;
    private readonly List<TableRow> _rows = []; 

    public BorderValue Border => _border;

    public TvBounds Bounds { get; set; }
    
    public ReadOnlySpan<TableRow> Rows => CollectionsMarshal.AsSpan(_rows);

    public Table(BorderValue border)
    {
        _border = border;
    }
    public TableRow AddRow()
    {
        var row = new TableRow();
        _rows.Add(row);
        return row;
    }
}