using Tvision2.Core;

namespace Tvision2.Engine.Components;

public readonly struct DrawResult
{
    private static DrawResult _done = new DrawResult(TvPoint.Zero, TvBounds.FromRowsAndCols(0, 0));
    public static DrawResult Done { get => _done; }
    public TvPoint Displacement { get; }

    public TvBounds BoundsAdjustment { get; }

    public DrawResult(in TvPoint displacement, in TvBounds adjustment)
    {
        Displacement = displacement;
        BoundsAdjustment = adjustment;
    }

    public static bool operator ==(in DrawResult one, in DrawResult two)
    {
        return one.Displacement == two.Displacement && one.BoundsAdjustment == two.BoundsAdjustment;
    }

    public static bool operator !=(in DrawResult one, in DrawResult two) => !(one == two);
}