using Tvision2.Core;
using Tvision2.Core.Console;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Tests;

[Trait("Category", "Console")]
public class Given_TextDrawn_In_VirtualConsole
{
    [Fact]
    public void Then_Text_Should_Be_In_Correct_Position()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var attr = new CharacterAttribute(TvColor.Blue, TvColor.Green, CharacterAttributeModifiers.Normal);
        var viewzone = new Viewzone(TvPoint.Zero, bounds);
        var stringToWrite = "Testing VirtualConsole";
        
        console.DrawAt(stringToWrite, TvPoint.FromXY(3,2), attr, viewzone);
        console.Flush(driver);
        
        var charsWritten = driver.GetCharsOfLine(2,3,stringToWrite.Length);
        var strWritten = new string(charsWritten.ToArray());
        strWritten.Should().Be(stringToWrite);
    }

    [Fact]
    public void And_Smaller_Viewport_Then_Only_First_N_Characters_Should_Be_Written()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var attr = new CharacterAttribute(TvColor.Blue, TvColor.Green, CharacterAttributeModifiers.Normal);
        var viewCols = 5;
        var viewzone = new Viewzone(TvPoint.FromXY(3,2), TvBounds.FromRowsAndCols(1, viewCols));
        var stringToWrite = "Testing VirtualConsole";
        var stringExpected = stringToWrite.Substring(0, viewCols);
        
        console.DrawAt(stringToWrite, TvPoint.FromXY(3,2), attr, viewzone);
        console.Flush(driver);
        
        var charsWritten = driver.GetCharsOfLine(2,3,stringToWrite.Length);
        var strWritten = new string(charsWritten.ToArray());
        strWritten.Should().StartWith(stringExpected);
    }

}