using Tvision2.Core;

namespace Tvision2.Console.Colors;

public interface IColorTranslator
{
    /// <summary>
    /// Given a RGB color and a Palette, returns the color of the palette
    /// that ost ressembles the RGB color.
    /// If no valid color is found returns null
    /// </summary>
    public TvColor? GetPalettizedColorFromRgb(TvColor color, IndexedPalette palette);
}