using Tvision2.Console;
using Tvision2.Core;
using Tvision2.Drawing;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Shapes;

// Draw a box
TvConsole.Foreground = TvColor.FromHexString("#33AAFF");
TvConsole.Background = TvColor.FromHexString("#444444");
var box = new Box(left: 5, top: 2, rows: 7, columns: 40, 
    BorderValue.HorizontalVertical(BorderType.Single, BorderType.Double));
TvConsole.Draw(box);
TvConsole.Fill(box, "#dd4444");
// Center text on it
TvConsole.Write("Welcome to Sheldon Cooper's", box , TextPosition.Top().CenterHorizontally(row: 1));
TvConsole.Foreground = TvColor.LightRed;
TvConsole.Write("Fun with boxes!", box, TextPosition.Bottom().CenterHorizontally());

// Draw and gradient fill another box
TvConsole.Foreground = TvColor.FromHexString("#FFFFFF");
TvConsole.Background = TvColor.FromHexString("#666666");
var box2 = new Box(TvPoint.FromXY(10, 12), TvBounds.FromRowsAndCols(10, 12), BorderValue.Double());
TvConsole.Draw(box2);
TvConsole.Fill(box2, p => TvColor.FromRgb(0x0, p.X * 10, p.Y * 10));

var box3 = new Box(TvPoint.FromXY(30, 20), TvBounds.FromRowsAndCols(10, 20), BorderValue.Single());
TvConsole.Draw(box3);
TvConsole.Wrap("Yes! You can wrap text inside shapes! Yes it's really amazing!", box3);

Console.ReadLine();