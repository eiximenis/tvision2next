using System.Drawing;
using System.Numerics;
using Tvision2.Core;
using Tvision2.Core.Console;

namespace Tvision2.Console;

public class IndexedPalette : IPalette
{
    private readonly TvColor[] _colors;
    public bool IsFreezed { get; } = false;
    public int MaxColors { get; }
    public ColorMode ColorMode { get; } = ColorMode.Palettized;

    public IndexedPalette(int numColors)
    {
        _colors = new TvColor[numColors];
    }
    
    public TvColor this[int idx] => _colors[idx];
}