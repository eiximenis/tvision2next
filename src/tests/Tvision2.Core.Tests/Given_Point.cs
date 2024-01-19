namespace Tvision2.Core.Tests;

[Trait("Category", "Core")]
public class Given_Point
{

    [Theory]
    [InlineData(0,0)]
    [InlineData(24,12)]
    [InlineData(10,30)]
    public void Compared_To_Another_Point_Should_Be_True_If_Both_Coordinates_Are_Equal(int x, int y)
    {
        var point = TvPoint.FromXY(x, y);
        var anotherPoint = TvPoint.FromXY(x, y);

        point.Equals(anotherPoint).Should().BeTrue();
        var equals = point == anotherPoint;
        equals.Should().BeTrue();
    }
    
    [Fact]
    public void Added_To_Zero_Point_Then_The_Result_Should_Be_It_Self()
    {
        var point = TvPoint.FromXY(20, 30);
        var add = point + TvPoint.Zero;
        add.Should().Be(point);
    }

    [Fact]
    public void As_TopLeft_And_ItSelf_As_BottomRight_Then_Resulting_Bounds_Must_Have_One_Column_And_One_Row()
    {
        var point = TvPoint.FromXY(10, 30);
        var bounds = TvPoint.CalculateBounds(point, point);

        bounds.Height.Should().Be(1);
        bounds.Width.Should().Be(1);
    }

    [Fact]
    public void As_TopLeft_And_Another_As_BottomRight_In_Same_Row_Then_Resulting_Bounds_Must_Have_Desired_Size()
    {
        var topleft = TvPoint.FromXY(10, 30);
        var bottomRightSameRow = TvPoint.FromXY(12, 30);
        var bounds = TvPoint.CalculateBounds(topleft, bottomRightSameRow);

        bounds.Height.Should().Be(1);
        bounds.Width.Should().Be(3);
    }

    [Fact]
    public void As_TopLeft_And_Another_As_BottomRight_In_Same_Column_Then_Resulting_Bounds_Must_Have_Desired_Size()
    {
        var topleft = TvPoint.FromXY(10, 30);
        var bottomRightSameColumn = TvPoint.FromXY(10, 34);
        var bounds = TvPoint.CalculateBounds(topleft, bottomRightSameColumn);
        bounds.Height.Should().Be(5);
        bounds.Width.Should().Be(1);
    }
    
    [Fact]
    public void As_TopLeft_And_Another_As_BottomRight_Then_Resulting_Bounds_Must_Have_Desired_Size()
    {
        var topleft = TvPoint.FromXY(10, 30); 
        var bottomRightDifferentColAndRow = TvPoint.FromXY(13, 32);
        var bounds = TvPoint.CalculateBounds(topleft, bottomRightDifferentColAndRow);
        bounds.Height.Should().Be(3);
        bounds.Width.Should().Be(4);     
    }
}