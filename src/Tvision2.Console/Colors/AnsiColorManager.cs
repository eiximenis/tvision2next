using Tvision2.Core;
using Tvision2.Core.Console;

namespace Tvision2.Console.Colors;

public class AnsiColorManager
{ 
    private const int FG_OFFSET = 30;
    private const int BG_OFFSET = 40;
    private const int BOLD_OFFSET = 60;
    
    public CharacterAttribute DefaultAttribute { get; }
    public IColorProcessor? ForegroundProcessor { get; set; } = null;
    public IColorProcessor? BackgroundProcessor { get; set; } = null;
    
    public AnsiColorManager()
    {
        DefaultAttribute = BuildAttributeFor(TvColor.White, TvColor.Black, CharacterAttributeModifiers.Normal);
    }

    public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back,
        CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
    {
        return new CharacterAttribute(fore, back, attrs);
    }

    private (TvColor fore, TvColor back) DecodeColorsFromAttribute(CharacterAttribute attr)
    {
        return (attr.Foreground, attr.Background);
    }



    public string GetForegroundAttributeSequence(TvColor fore,
        CharacterAttributeModifiers modifiers = CharacterAttributeModifiers.Normal)
    {
        string? forestring;

        var newFore = ForegroundProcessor?.Process(fore, modifiers) ?? fore;
        if (fore.IsRgb)
        {
            var (fr, fg, fb) = newFore.Rgb;
            forestring = string.Format(AnsiEscapeSequences.SGR_RGB_FORE, fr.ToString(), fg.ToString(), fb.ToString());
        }

        else if (fore.IsPalettized)
        {
            forestring = string.Format(AnsiEscapeSequences.SGR_88_FORE, fore.PaletteIndex);
        }
        else
        {
            var boldOffset = modifiers.HasFlag(CharacterAttributeModifiers.Bold) ? BOLD_OFFSET : 0;
            forestring = string.Format(AnsiEscapeSequences.SGR_ANSI, fore.Value + FG_OFFSET + boldOffset);
        }

        return forestring;
    }

    public string GetBackgroundAttributeSequence(TvColor back,
        CharacterAttributeModifiers modifiers = CharacterAttributeModifiers.Normal)
    {
        var newBack = BackgroundProcessor?.Process(back, modifiers) ?? back;
        string? backstring;
        if (back.IsRgb)
        {
            var (br, bg, bb) = newBack.Rgb;
            backstring = string.Format(AnsiEscapeSequences.SGR_RGB_BACK, br.ToString(), bg.ToString(), bb.ToString());
        }

        else if (back.IsPalettized)
        {
            backstring = string.Format(AnsiEscapeSequences.SGR_88_BACK, back.PaletteIndex);
        }
        else
        {
            var boldOffset = modifiers.HasFlag(CharacterAttributeModifiers.BackgroundBold) ? BOLD_OFFSET : 0;
            backstring = string.Format(AnsiEscapeSequences.SGR_ANSI, back.Value + BG_OFFSET + boldOffset);
        }

        return backstring;
    }
    
    public string GetAttributeSequence(CharacterAttribute attributes)
    {
        var (fore, back) = DecodeColorsFromAttribute(attributes);
        return GetForegroundAttributeSequence(fore, attributes.Modifiers) +
               GetBackgroundAttributeSequence(back, attributes.Modifiers);
    }

    public string GetCursorSequence(int x, int y)
    {
        return string.Format(AnsiEscapeSequences.CUP, y + 1, x + 1);
    }
}
