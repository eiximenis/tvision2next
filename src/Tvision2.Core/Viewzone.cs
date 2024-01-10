namespace Tvision2.Core;

public readonly record struct Viewzone(TvPoint TopLeft, TvBounds Bounds)
{
    public static Viewzone FromTopLeftToBottomRight(TvPoint topLeft, TvPoint bottomRight) =>
        new Viewzone(topLeft, TvPoint.CalculateBounds(topLeft, bottomRight));
    
    public TvPoint BottomRight => TopLeft + Bounds;
    public static Viewzone FromBounds(TvBounds bounds) => new Viewzone(TvPoint.Zero, bounds);
    
        public static Viewzone CropHorizontally(in  Viewzone container, in Viewzone toCrop)
    {
        var containerBottomRight = container.BottomRight;
        var toCropBottomRight = toCrop.BottomRight;
        var leftCol = container.TopLeft.X > toCrop.TopLeft.X ? container.TopLeft.X : toCrop.TopLeft.X;
        var rightCol = containerBottomRight.X < toCropBottomRight.X ? containerBottomRight.X : toCropBottomRight.X;
        return new Viewzone(TvPoint.FromXY(leftCol, toCrop.TopLeft.Y),
            TvBounds.FromRowsAndCols(toCrop.Bounds.Height, rightCol - leftCol + 1));
    }

    public static Viewzone CropVertically(in Viewzone container, in Viewzone toCrop)
    {
        var containerBottomRight = container.BottomRight;
        var toCropBottomRight = toCrop.BottomRight;

        var topRow = container.TopLeft.Y > toCrop.TopLeft.Y ? container.TopLeft.Y : toCrop.TopLeft.Y;
        var bottomRow = containerBottomRight.Y < toCropBottomRight.Y ? containerBottomRight.Y : toCropBottomRight.Y;

        return new Viewzone(TvPoint.FromXY(toCrop.TopLeft.X, topRow),
            TvBounds.FromRowsAndCols(bottomRow - topRow + 1, toCrop.Bounds.Width));
    }
        
    public static Viewzone Crop(in Viewzone container, in Viewzone toCrop)
    {
        var containerBottomRight = container.BottomRight;
        var toCropBottomRight = toCrop.BottomRight;
        var leftCol = container.TopLeft.X > toCrop.TopLeft.X ? container.TopLeft.X : toCrop.TopLeft.X;
        var rightCol = containerBottomRight.X < toCropBottomRight.X ? containerBottomRight.X : toCropBottomRight.X;
        var topRow = container.TopLeft.Y > toCrop.TopLeft.Y ? container.TopLeft.Y : toCrop.TopLeft.Y;
        var bottomRow = containerBottomRight.Y < toCropBottomRight.Y ? containerBottomRight.Y : toCropBottomRight.Y;

        var topLeft = TvPoint.FromXY(leftCol, topRow);
        var bottomRight = TvPoint.FromXY(rightCol, bottomRow);
        return new Viewzone(topLeft, TvPoint.CalculateBounds(topLeft, bottomRight));
    }
    
    public bool ContainsLine(int row)
    {
        return TopLeft.Y <= row && BottomRight.Y >= row;
    }
    
    public bool ContainsColumn(int col)
    {
        return TopLeft.X <= col && BottomRight.X >= col;
    }
}