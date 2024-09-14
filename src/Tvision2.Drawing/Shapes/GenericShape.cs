using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;

namespace Tvision2.Drawing.Shapes;
public class GenericShape : IShape
{
    private readonly Func<TvPoint, bool> _pointIsInside;
    public GenericShape(TvPoint topLeft, TvBounds bounds, Func<TvPoint, bool> pointIsInside)
    {
        TopLeft = topLeft;
        Bounds = bounds;
        _pointIsInside = pointIsInside;
    }
    public bool PointIsInside(TvPoint point) => _pointIsInside.Invoke(TopLeft - point);

    public TvPoint TopLeft { get; }
    public TvBounds Bounds { get; }
    public TvPoint TopLeftInside => TopLeft;
    public TvPoint BottomRightInside => TopLeft + Bounds;
}
