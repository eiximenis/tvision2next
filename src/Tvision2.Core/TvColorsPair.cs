namespace Tvision2.Core;

public readonly ref struct TvColorsPair
{
    public TvColor Foreground { get; }
    public TvColor Background { get; }
    private TvColorsPair(TvColor fore, TvColor back)
    {
        Foreground = fore;
        Background = back;
    }

    public static TvColorsPair FromForegroundAndBackground(TvColor fore, TvColor back) => new TvColorsPair(fore, back);

    public static TvColorsPair FromForegroundAndBackground(string hexFore, TvColor back) =>
        new TvColorsPair(TvColor.FromHexString(hexFore), back);
        
    public static TvColorsPair FromForegroundAndBackground(TvColor fore, string hexBack) =>
        new TvColorsPair(fore, TvColor.FromHexString(hexBack));

    public static TvColorsPair FromForegroundAndBackground(string hexFore, string hexBack) =>
        new TvColorsPair(TvColor.FromHexString(hexFore), TvColor.FromHexString(hexBack));
    
}