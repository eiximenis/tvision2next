using Tvision2.Core;
using Tvision2.Core.Console;

namespace Tvision2.Console.Colors;

public class PalettizedColorProcessor : IColorProcessor
{
    private readonly IndexedPalette _palette;
    private readonly IColorTranslator _translator;
    public PalettizedColorProcessor(IndexedPalette palette, IColorTranslator? translator = null)
    {
        _palette = palette;
        _translator = translator;
    }
    public TvColor Process(TvColor original, CharacterAttributeModifiers attributtes)
    {
        if (original.IsPalettized || original.IsBasic) return original;
        
        // If color is RGB and application mode is palettized we need to use a color translator
        // to translate the RGB color to a palette color. If not set, then a exception is thrown

        return _translator?.GetPalettizedColorFromRgb(original, _palette) ??
               throw new ArgumentException(
                   "A RGB color has been passed but current color mode is palettized and no color translator has been set");
    }
}

public class TrueColorProcessor : IColorProcessor
{
    public TvColor Process(TvColor original, CharacterAttributeModifiers attributtes)
    {
        if (original.IsRgb) return original;

        if (original.IsPalettized)
            throw new ArgumentException("Color mode is RGB and a palettized color has been found!!!!");
        
        return original.PaletteIndex switch
            {
                TvColorNames.Black => TvColor.FromRgb(0, 0, 0),
                TvColorNames.Red => TvColor.FromRgb(170, 0, 0),
                TvColorNames.Green => TvColor.FromRgb(0, 170, 0),
                TvColorNames.Yellow => TvColor.FromRgb(170, 85, 0),
                TvColorNames.Blue => TvColor.FromRgb(0, 0, 170),
                TvColorNames.Magenta => TvColor.FromRgb(170, 0, 170),
                TvColorNames.Cyan => TvColor.FromRgb(0, 170, 170),
                TvColorNames.White => TvColor.FromRgb(170, 170, 170),
                TvColorNames.LightBlack => TvColor.FromRgb(85, 85, 85),
                TvColorNames.LightRed => TvColor.FromRgb(255, 85, 85),
                TvColorNames.LightGreen => TvColor.FromRgb(85, 255, 85),
                TvColorNames.LightYellow => TvColor.FromRgb(255, 255, 85),
                TvColorNames.LightBlue => TvColor.FromRgb(85, 85, 255),
                TvColorNames.LightMagenta => TvColor.FromRgb(255, 85, 255),
                TvColorNames.LightCyan => TvColor.FromRgb(85, 255, 255),
                TvColorNames.LightWhite => TvColor.FromRgb(255, 255, 255),
                _ => throw new ArgumentException($"Basic color with raw value {original.PaletteIndex} is not valid!")
            };
    }
}