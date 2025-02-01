using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks.Sources;
using HelloTvConsole;
using Tvision2.Console;
using Tvision2.Core;
using Tvision2.Drawing;
using Tvision2.Drawing.Borders;
using Tvision2.Drawing.Shapes;
using Tvision2.Drawing.Tables;
using Tvision2.Drawing.Text;

// Draw a box
TvConsole.Foreground = TvColor.FromHexString("#FF0000");
TvConsole.Background = TvColor.FromHexString("#444444");
var box = new Box(left: 5, top: 2, rows: 7, columns: 40, 
    BorderValue.HorizontalVertical(BorderType.Single, BorderType.Double));
TvConsole.Draw(box);
TvConsole.Fill(box, "#00ff00");
// Center text on it
TvConsole.Write("Welcome to Sheldon Cooper's", box , TextPosition.Top().Center());
TvConsole.Write("Text in the middle? No brainer!", box, TextPosition.Middle().Center());
TvConsole.Foreground = TvColor.LightRed;
TvConsole.Write("Fun with boxes!", box, TextPosition.Bottom().Center());

// Draw and gradient fill another box
TvConsole.Foreground = TvColor.FromHexString("#FFFFFF");
TvConsole.Background = TvColor.FromHexString("#666666");
var box2 = new Box(TvPoint.FromXY(10, 12), TvBounds.FromRowsAndCols(6, 100), BorderValue.Double());
TvConsole.Draw(box2);
TvConsole.Fill(box2, p => TvColor.FromRgb(p.Y < 2 ? 0x0 : p.Y * 40, p.X * 2, (p.X * p.Y) % 256));

// Text wrapping inside boxes
TvConsole.Foreground = TvColor.FromHexString("#ddaabb");
TvConsole.Background = TvColor.FromHexString("#223322");
var boxNone = new Box(TvPoint.FromXY(10, 20), TvBounds.FromRowsAndCols(7, 20), BorderValue.Single());
TvConsole.Draw(boxNone);
TvConsole.Wrap("Yes! You can wrap text inside shapes! Yes it's really amazing!", boxNone);

var box3 = new Box(TvPoint.FromXY(30, 20), TvBounds.FromRowsAndCols(7, 20), BorderValue.Single());
TvConsole.Draw(box3);
TvConsole.Wrap("👉👉 Text can be aligned to the left!", box3, Justification.Left);

var box4 = new Box(TvPoint.FromXY(50, 20), TvBounds.FromRowsAndCols(7, 20), BorderValue.Double());
TvConsole.Draw(box4);
TvConsole.Wrap("Of course it's possible to align text to the right! 😉", box4, Justification.Right);

var box5 = new Box(TvPoint.FromXY(70, 20), TvBounds.FromRowsAndCols(7, 20), BorderValue.HorizontalVertical(BorderType.Double, BorderType.Single));
TvConsole.Draw(box5);
TvConsole.Wrap("Or maybe (only maybe) you want full justification 😊 So Easy!", box5, Justification.Full);
var box6 = new Box(TvPoint.FromXY(90, 20), TvBounds.FromRowsAndCols(10, 25), BorderValue.HorizontalVertical(BorderType.Single, BorderType.Double));
TvConsole.Draw(box6, new BlueRangeColor(box6.TopLeft ,6));
TvConsole.Wrap("Or maybe you want to center text 🤷! Everything is possible!", box6, Justification.Center);

Console.ReadLine();
Console.Clear();

var ttext = new TabulatedText();
ttext.AddEntry("Line 1", "First Item", 30.1);
ttext.AddEntry("Line 2", "Second Item\nExtra line with extra data\u2028Another extra line here!", 28);
ttext.AddEntry("Line 3", "First Item", 30.2);
TvConsole.Tabulate(ttext);

var table = new Table(BorderValue.Single());
table.AddRow(RowHeight.Fixed(1)).AddCells(2);
table.AddRow(RowHeight.Relative(3))
    .AddCell(ColumnWidth.Fixed(10))
    .AddCells(2);
table.AddRows(3).AddCells(4);
table.AddRow(RowHeight.Relative(5)).AddCells(1);
table.Bounds = TvBounds.FromRowsAndCols(21, 50);

var pos = TvPoint.FromXY(0, 7);
TvConsole.Draw(table, pos);
var cell11 = table.GetCellBox(pos, 1, 1);
TvConsole.Wrap("Ufo", cell11, Justification.Center);

var cell01 = table.GetCellBox(pos, 2, 1);
TvConsole.Fill(cell01, new BlueRangeColor(cell01.TopLeftInside, 20));

var cell40 = table.GetCellBox(pos, 4, 0);
TvConsole.Fill(cell40, TvColor.FromHexString("#ddaa90"));
var cell50 = table.GetCellBox(pos, 5, 0);
var table2 = new Table(BorderValue.Single());
table2.AddRow(RowHeight.Fixed(1)).AddCells(3);
table2.AddRow(RowHeight.Fixed(1)).AddCells(2);
table2.Bounds = cell50.Bounds.Reduced(TvBounds.FromRowsAndCols(2, 2));
TvConsole.Draw(table2, cell50.TopLeftInside + TvPoint.FromXY(1, 1));

Console.ReadKey();


