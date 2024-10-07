using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Drawing.Borders;
public class BorderSets
{
    private static readonly Dictionary<BorderValue, char[]> _characters;

    public static class Entries
    {
        public const int TOPLEFT = 0;
        public const int HORIZONTAL = 1;
        public const int TOPRIGHT = 2;
        public const int VERTICAL = 3;
        public const int BOTTOMLEFT = 4;
        public const int BOTTOMRIGHT = 5;
        public const int HORIZONTALUP = 6;
        public const int HORIZONTALDOWN = 7;
        public const int CROSS = 8;
        public const int VERTICALLEFT = 9;
        public const int VERTICALRIGHT = 10;
    }

    static BorderSets()
    {
        _characters = new Dictionary<BorderValue, char[]>();
        FillCharacters();
    }

    private static void FillCharacters()
    {
        _characters.Add(BorderValue.Double(), ['\u2554', '\u2550', '\u2557', '\u2551', '\u255a', '\u255d', '\u2566', '\u2569', '\u256c', '\u2560', '\u2563']);
        _characters.Add(BorderValue.Single(), ['\u250c', '\u2500', '\u2510', '\u2502', '\u2514', '\u2518', '\u252c', '\u2534', '\u253c', '\u2520', '\u2525']);
        _characters.Add(BorderValue.Spaces(), [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Double, BorderType.Single), ['\u2552', '\u2550', '\u2555', '\u2502', '\u2558', '\u255b', '\u2567', '\u2564', '\u256a', '\u255e', '\u2561']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Single, BorderType.Double), ['\u2553', '\u2500', '\u2556', '\u2551', '\u2559', '\u255c', '\u2568', '\u2565', '\u256b', '\u255f', '\u2562']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Fill, BorderType.Fill), ['\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Fill, BorderType.Single), ['\u250c', '\u2591', '\u2510', '\u2502', '\u2514', '\u2518', '\u252c', '\u2534', '\u253c', '\u2500', '\u2500']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Fill, BorderType.Double), ['\u2554', '\u2591', '\u2557', '\u2551', '\u255a', '\u255d', '\u2566', '\u2569', '\u256c', '\u2550', '\u2550']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Double, BorderType.Fill), ['\u2554', '\u2550', '\u2557', '\u2591', '\u255a', '\u255d', '\u2566', '\u2569', '\u256c', '\u2591', '\u2591']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Single, BorderType.Fill), ['\u250c', '\u2500', '\u2510', '\u2591', '\u2514', '\u2518', '\u252c', '\u2534', '\u253c', '\u2591', '\u2591']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Double, BorderType.None), ['\u2550', '\u2550', '\u2550', '\0', '\u2550', '\u2550', '\u2550', '\u2550', '\u2550', '\0', '\0']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Single, BorderType.None), ['\u2500', '\u2500', '\u2500', '\0', '\u2500', '\u2500', '\u2500', '\u2500', '\u2500', '\0', '\0']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Fill, BorderType.None), ['\u2591', '\u2591', '\u2591', '\0', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\0', '\0']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.None, BorderType.Double), ['\0', '\0', '\0', '\u2551', '\0', '\0', '\0', '\0', '\u2551', '\u2550', '\u2550']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.None, BorderType.Single), ['\0', '\0', '\0', '\u2502', '\0', '\0', '\0', '\0', '\u2502', '\u2500', '\u2500']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.None, BorderType.Fill), ['\0', '\0', '\0', '\u2591', '\0', '\0', '\0', '\0', '\u2591', '\u2591', '\u2591']);

        _characters.Add(BorderValue.HorizontalVertical(BorderType.Double, BorderType.Spaces), ['\u2550', '\u2550', '\u2550', ' ', '\u2550', '\u2550', '\u2550', '\u2550', '\u2550', ' ', ' ']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Single, BorderType.Spaces), ['\u2500', '\u2500', '\u2500', ' ', '\u2500', '\u2500', '\u2500', '\u2500', '\u2500', ' ', ' ']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Fill, BorderType.Spaces), ['\u2591', '\u2591', '\u2591', ' ', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', ' ', ' ']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Spaces, BorderType.Double), [' ', ' ', ' ', '\u2551', ' ', ' ', ' ', ' ', '\u2551', '\u2550', '\u2550']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Spaces, BorderType.Single), [' ', ' ', ' ', '\u2502', ' ', ' ', ' ', ' ', '\u2502', '\u2500', '\u2500']);
        _characters.Add(BorderValue.HorizontalVertical(BorderType.Spaces, BorderType.Fill), [' ', ' ', ' ', '\u2591', ' ', ' ', ' ', ' ', '\u2591', '\u2591', '\u2591']);


    }

    public static char[] GetBorderSet(BorderValue value) => _characters.TryGetValue(value, out var characters) ? characters : _characters[BorderValue.Double()];
}


