using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Core;
public readonly record struct Margin(int Left = 0, int Right = 0, int Top = 0, int Bottom = 0)
{
    public static Margin None => new ();
    public static Margin FromValue(int value) => new Margin(value, value, value, value);
    public static Margin LeftMargin(int value) => new Margin(Left: value);

    public static Margin RightMargin(int value) => new Margin(Right: value);
    public static Margin TopMargin(int value) => new Margin(Top: value);
    public static Margin BottomMargin(int value) => new Margin(Bottom: value);
    public static Margin Vertical(int value) => new Margin(Left: value, Right: value);
    public static Margin Horizontal(int value) => new Margin(Top: value, Right: value);
    public static Margin HorizontalAndVertical(int hor, int ver) =>
        new Margin(Top: hor, Bottom: hor, Left: ver, Right: ver);

    public bool IsNone => Left == 0 && Right == 0 && Top == 0 && Bottom == 0;
}
