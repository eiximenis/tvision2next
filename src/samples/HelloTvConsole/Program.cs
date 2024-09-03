// See https://aka.ms/new-console-template for more information

using Tvision2.Console;
using Tvision2.Console.Boxes;
using Tvision2.Core;
using Tvision2.Drawing;

// Draw a nice background :)
for (int row = 0; row < Console.WindowHeight; row++)
{
    for (int col = 0; col < Console.WindowWidth; col++)
    {
        var color = TvColor.FromRgb((col +row) % 256, col % 256, (col * 2) % 256);
        TvConsole.Background = color;
        TvConsole.Write(" ");
    }
    TvConsole.WriteLine("");
}

// Draw a box
TvConsole.Foreground = TvColor.FromHexString("#33AAFF");
TvConsole.Background = TvColor.FromHexString("#444444");
var box = new Box(left: 5, top: 2, rows: 7, columns: 30);
TvConsole.Draw(box);
TvConsole.Fill(box, "#dd4444");
// Center text on it
TvConsole.Write("Boxes made easy", box , TextPosition.CenterHorizontally(row: 1));
TvConsole.Write("Really easy!", box, TextPosition.Bottom().CenterHorizontally());


Console.ReadLine();