namespace Tvision2.Core.Console;

public enum ColorMode
{
    Basic,
    Palettized,
    Direct
}

public interface IPalette
{
    bool IsFreezed { get; }
    int MaxColors { get; }
    ColorMode ColorMode { get; }
}

