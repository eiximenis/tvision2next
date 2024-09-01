// See https://aka.ms/new-console-template for more information

using Tvision2.Console;
using Tvision2.Core;


for (int col = 0; col < 64; col++)
{
    for (int row = 0; row < 64; row++)
    {
        var color = TvColor.FromRgb(col * 4, row * 4, row * 4);
        TvConsole.Background = color;
        TvConsole.Write(" ");
    }
    TvConsole.WriteLine("");
}

TvConsole.Foreground = TvColor.FromHexString("#33AAFF");
TvConsole.Background = TvColor.FromHexString("#444444");
TvConsole.DrawBox(left: 10, top: 2, rows: 4, columns: 19);
TvConsole.Write("Boxes made easy",left: 11, top: 3);

Console.ReadLine();