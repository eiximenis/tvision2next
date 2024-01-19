namespace Tvision2.Core.Tests;

[Trait("Category", "Core")]
public class Given_Color
{
    [Theory]
    [InlineData(0,0,0)]
    [InlineData(128,128,255)]
    [InlineData(0,90,22)]
    [InlineData(255,255,255)]
    public void Built_With_Some_RGB_Values_Then_Resulting_Color_Should_Be_RGB_With_Valid_Components(byte expectedRed, byte expectedGreen, byte expectedBlue)
    {
        var color = TvColor.FromRgb(expectedRed, expectedGreen, expectedBlue);
        color.IsRgb.Should().BeTrue();
        var (red, green, blue) = color.Rgb;
        red.Should().Be(expectedRed);
        green.Should().Be(expectedGreen);
        blue.Should().Be(expectedBlue);        
    }


    [Theory]
    [InlineData("#ff00aa", 0xff, 0x0, 0xaa)]
    [InlineData("#123456", 0x12, 0x34, 0x56)]
    public void Built_With_Hex_String_Then_Resulting_Color_Should_Be_RGB_With_Valid_Components(string hex,
        byte expectedRed, byte expectedGreen, byte expectedBlue)
    {
        var color = TvColor.FromHexString(hex);
        color.IsRgb.Should().BeTrue();
        var (red, green, blue) = color.Rgb;
        red.Should().Be(expectedRed);
        green.Should().Be(expectedGreen);
        blue.Should().Be(expectedBlue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(128)]
    [InlineData(255)]
    public void Built_Using_Palette_Index_Then_Resulting_Color_Should_Be_Palettized_With_Valid_Index(byte idx)
    {
        var color = TvColor.FromPaletteIndex(idx);
        color.IsPalettized.Should().BeTrue();
        color.PaletteIndex.Should().Be(idx);
    }

    [Fact]
    public void Built_Using_Palette_Index_In_Grey_Range_Then_Resulting_Color_Should_Be_Palettized_And_Grey()
    {
        var gray = TvColor.FromPaletteIndex(TvColor.PALETTIZED_GRAY_START_IDX + 1);
        gray.IsGray.Should().BeTrue();

        var noGray = TvColor.FromPaletteIndex(TvColor.PALETTIZED_GRAY_START_IDX - 1);
        noGray.IsGray.Should().BeFalse();
    }

    [Fact]
    public void Retrieved_Using_Predefined_Colors_Then_Retrieved_Color_Should_Be_Basic()
    {
        var black = TvColor.Black;
        black.IsBasic.Should().BeTrue();
        var lcyan = TvColor.LightCyan;
        lcyan.IsBasic.Should().BeTrue();
    }

    [Fact]
    public void That_Is_A_Basic_Color_Then_It_Should_Be_Palettized_Too()
    {
        var black = TvColor.Black;
        black.IsPalettized.Should().BeTrue();
        var lcyan = TvColor.LightCyan;
        lcyan.IsPalettized.Should().BeTrue();
    }

    [Fact]
    public void That_Is_Palettized_Then_It_Should_Not_Be_RGB()
    {
        var black = TvColor.Black;
        black.IsRgb.Should().BeFalse();
        var somePaletteColor = TvColor.FromPaletteIndex(100);
        somePaletteColor.IsRgb.Should().BeFalse();
    }

    [Theory]
    [InlineData(0,0,10)]
    [InlineData(100,200,50)]
    [InlineData(255,255,255)]
    public void That_Is_RGB_Then_It_Should_Be_Equal_To_Another_RGB_Color_With_Same_Components(byte r, byte b, byte g)
    {
        var first = TvColor.FromRgb(r,g,b);
        var second = TvColor.FromRgb(r,g,b);
        first.Should().Be(second);
    }
    
}