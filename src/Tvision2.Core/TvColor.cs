namespace Tvision2.Core;

public static class TvColorNames
{
    private static readonly string[] _names = ["Black", "Red", "Green", "Yellow", "Blue", "Magenta", "Cyan", "White", "Light Black", "Light Red", "Light Green", "Light Yellow", "Light Blue", "Light Magenta", "Light Cyan", "Light White"];
    public const int Black = 0;
    public const int Red = 1;
    public const int Green = 2;
    public const int Yellow = 3;
    public const int Blue = 4;
    public const int Magenta = 5;
    public const int Cyan = 6;
    public const int White = 7;
    public const int LightBlack = 8;
    public const int LightRed = 9;
    public const int LightGreen = 10;
    public const int LightYellow = 11;
    public const int LightBlue = 12;
    public const int LightMagenta = 13;
    public const int LightCyan = 14;
    public const int LightWhite = 15;

    public const int StandardColorsCount = 8;
    public static IEnumerable<string> AlLStandardColorNames => _names;

    public static string NameOf(int value)
    {
        return _names[(value % _names.Length + 1) - 1];
    }

    public static string NameOf(TvColor color) => NameOf(color.Value);
}

    public enum TvPalettizedComponentValue : short
    {
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five
    }

    public readonly struct TvColor
    {
        public readonly int Value;
        private const int RGB_MARKER = (1 << 31);
        private const int PALETTE_MARKER = (1 << 30);
        private const int ANSI3BIT_MAX_VALUE = 7;
        public const int ANSI4BIT_MAX_VALUE = 15;

        public const short PALETTIZED_GRAY_START_IDX = 232;
        public const short PALETTIZED_GRAY_RANGE = 24;
        private TvColor(int value) => Value = value;
        public static readonly TvColor Black = new TvColor(TvColorNames.Black);
        public static readonly TvColor Red = new TvColor(TvColorNames.Red);
        public static readonly TvColor Green = new TvColor(TvColorNames.Green);
        public static readonly TvColor Yellow = new TvColor(TvColorNames.Yellow);
        public static readonly TvColor Blue = new TvColor(TvColorNames.Blue);
        public static readonly TvColor Magenta = new TvColor(TvColorNames.Magenta);
        public static readonly TvColor Cyan = new TvColor(TvColorNames.Cyan);
        public static readonly TvColor White = new TvColor(TvColorNames.White);
        public static readonly TvColor LightBlack = new TvColor(TvColorNames.LightBlack);
        public static readonly TvColor LightRed = new TvColor(TvColorNames.LightRed);
        public static readonly TvColor LightGreen = new TvColor(TvColorNames.LightGreen);
        public static readonly TvColor LightYellow = new TvColor(TvColorNames.LightYellow);
        public static readonly TvColor LightBlue = new TvColor(TvColorNames.LightBlue);
        public static readonly TvColor LightMagenta = new TvColor(TvColorNames.LightMagenta);
        public static readonly TvColor LightCyan = new TvColor(TvColorNames.LightCyan);
        public static readonly TvColor LightWhite = new TvColor(TvColorNames.LightWhite);
        
        public static explicit operator short(TvColor color) => (short)color.Value;
        public static bool operator ==(TvColor one, TvColor other) => one.Value == other.Value;
        public static bool operator !=(TvColor one, TvColor other) => one.Value != other.Value;

        public static bool operator ==(TvColor one, int other) => one.Value == other;
        public static bool operator !=(TvColor one, int other) => one.Value == other;

        // Colors 0-15 are considered ansi basic colors
        public bool IsBasic => Value <= ANSI4BIT_MAX_VALUE;
        // Ansi basic colors between 8-15 are considered "bright" colors
        public bool IsBright => IsBasic && Value > ANSI3BIT_MAX_VALUE;
        // A palettized color is just an index (0-255) over a palette. Used in 256 color mode.
        public bool IsPalettized => IsBasic || (Value & PALETTE_MARKER) != 0;
        // A Palettized color with index >= 232 is considered a "gray color"
        public bool IsGray => IsPalettized && (PaletteIndex > PALETTIZED_GRAY_START_IDX);
        // A RGB color is a 24-bit color, used in true color mode
        public bool IsRgb => (Value & RGB_MARKER) != 0;
        
        /// <summary>
        /// Builds a RGB color with specified values for red, green and blue
        /// </summary>
        public static TvColor FromRgb(byte red, byte green, byte blue)
        {
            var value = red | (green << 8) | (blue << 16) | RGB_MARKER;
            return new TvColor(value);
        }
        public static TvColor FromRgb(int red, int green, int blue) => FromRgb((byte)red, (byte)green, (byte)blue);
        
    
        /// <summary>
        /// Builds a palettized color with specified values of red, green and blue.
        /// In palettized color mode, RGB values are 0-6. The result color will have "IsGray" property to false.
        /// </summary>
        /// <returns></returns>
        public static TvColor FromPaletteValues(TvPalettizedComponentValue red, TvPalettizedComponentValue green, TvPalettizedComponentValue blue) 
            => FromPaletteIndex((byte)(16 + 36 * (short)red + 6 * (short)green + (short)blue));
        
        /// <summary>
        /// Builds a palettized gray color. The resulting color will have "IsGray" propery to true.
        /// </summary>
        public static TvColor FromPaletteGrey(short greyIndex)
            => FromPaletteIndex((byte)(PALETTIZED_GRAY_START_IDX + (greyIndex > PALETTIZED_GRAY_RANGE ? PALETTIZED_GRAY_RANGE : greyIndex)));

        /// <summary>
        /// Builds a palettized color with specified index.
        /// </summary>
        public static TvColor FromPaletteIndex(byte index)
        {
            var value = (int)index | PALETTE_MARKER;
            return new TvColor(value);
        }

        /// <summary>
        /// Builds a RGB color from hex string (like #aaff34) with RGB values
        /// </summary>
        public static TvColor FromHexString(string hexValue)
        {
            var start = hexValue[0] == '#' ? 1 : 0;
            var sred = hexValue.AsSpan(start, 2);
            var sgreen = hexValue.AsSpan(start + 2, 2);
            var sblue = hexValue.AsSpan(start + 4, 2);
            var red = byte.Parse(sred, System.Globalization.NumberStyles.HexNumber);
            var green = byte.Parse(sgreen, System.Globalization.NumberStyles.HexNumber);
            var blue = byte.Parse(sblue, System.Globalization.NumberStyles.HexNumber);
            return TvColor.FromRgb(red, green, blue);
        }

        /// <summary>
        /// Returns the index of the color in the current palette.
        /// If color is not basic nor palettized, then a -1 is returned.
        /// </summary>
        public int PaletteIndex => (IsPalettized  || IsBasic) ? Value & ~PALETTE_MARKER : -1;

        /// <summary>
        /// Returns RGB components of a RGB color.
        /// If color is not RGB return value is not defined
        /// </summary>
        public (byte red, byte green, byte blue) Rgb =>
            ((byte)(Value & 0xff), (byte)(Value >> 8), (byte)(Value >> 16));

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                TvColor c => c.Value == Value,
                int v => v == Value,
                _ => false
            };
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            if (IsRgb)
            {
                var (r, g, b) = Rgb;
                return $"RGB({r},{g},{b}) ({Value})";
            }

            if (IsPalettized)
            {
                return $"PAL({PaletteIndex}) ({Value})";
            }

            if (Value < TvColorNames.StandardColorsCount)
            {

                return $"STD({TvColorNames.NameOf(Value)}) ({Value})";
            }

            return $"???({Value})";

        }

        public (int red, int green, int blue) Diff(TvColor other)
        {
            var rgb = this.Rgb;
            var otherRgb = other.Rgb;
            return (rgb.red - otherRgb.red, rgb.green - otherRgb.green, rgb.blue - otherRgb.blue);
        }
    }