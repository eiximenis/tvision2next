using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Tests;

[Trait("Category", "Components Tree")]
public class Given_ComponentTree
{
    [Fact]
    public void When_A_Component_Is_Added_As_Root_Then_It_Should_Be_Returned_In_Roots_Collection()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);

        TvComponent c1 = TvComponent.CreateStatelessComponent();
        ui.ComponentTree.Add(c1);

        ui.ComponentTree.Roots.Should().HaveCount(1);
        ui.ComponentTree.Roots.Should().ContainSingle(item => item == c1.Metadata.Node);
    }

    [Fact]
    public void When_A_Component_Is_Added_As_A_Child_Then_It_Should_Not_Be_Returned_In_Roots_Collection()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);

        TvComponent root = TvComponent.CreateStatelessComponent();
        TvComponent child = TvComponent.CreateStatelessComponent();
        ui.ComponentTree.Add(root);
        ui.ComponentTree.AddChild(child, root);
        
        ui.ComponentTree.Roots.Should().HaveCount(1);
        ui.ComponentTree.Roots.Should().NotContain(item => item == child.Metadata.Node);
    }
}