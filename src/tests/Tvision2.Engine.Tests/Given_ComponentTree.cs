using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Tests;

[Trait("Category", "Components Tree")]
public class Given_ComponentTree
{
    [Fact]
    public async Task When_A_Component_Is_Added_As_Root_Then_It_Should_Be_Returned_In_Roots_Collection()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);

        TvComponent c1 = TvComponent.CreateStatelessComponent();
        await ui.ComponentTree.Add(c1);

        ui.ComponentTree.Roots.Should().HaveCount(1);
        ui.ComponentTree.Roots.Should().ContainSingle(item => item == c1.Metadata.Node);
    }

    [Fact]
    public async Task When_A_Component_Is_Added_As_A_Child_Then_It_Should_Not_Be_Returned_In_Roots_Collection()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);

        TvComponent root = TvComponent.CreateStatelessComponent();
        TvComponent child = TvComponent.CreateStatelessComponent();
        await ui.ComponentTree.Add(root);
        await ui.ComponentTree.AddChild(child, root);
        
        ui.ComponentTree.Roots.Should().HaveCount(1);
        ui.ComponentTree.Roots.Should().NotContain(item => item == child.Metadata.Node);
    }

    [Fact]
    public async Task When_A_Component_Is_Added_As_A_Child_Of_A_NodeThat_Is_Not_Yet_Add_Then_It_Should_Not_Be_Added()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);
        
        TvComponent root = TvComponent.CreateStatelessComponent();
        TvComponent child = TvComponent.CreateStatelessComponent();
        TvComponent child2 = TvComponent.CreateStatelessComponent();
        await ui.ComponentTree.AddChild(child, root);
        await ui.ComponentTree.AddChild(child2, root);
        
        ui.ComponentTree.Roots.Should().BeEmpty();
    }

    [Fact]
    public async Task  When_A_Root_Is_Added_Then_All_Its_Childs_Should_Be_Added_Too()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);
        
        TvComponent root = TvComponent.CreateStatelessComponent();
        TvComponent child = TvComponent.CreateStatelessComponent();
        TvComponent child2 = TvComponent.CreateStatelessComponent();
        await ui.ComponentTree.AddChild(child, root);
        await ui.ComponentTree.AddChild(child2, root);
        await ui.ComponentTree.Add(root);
        
        ui.ComponentTree.Roots.Should().HaveCount(1);
        ui.ComponentTree.Roots.Should().ContainSingle(item => item == root.Metadata.Node);
        ui.ComponentTree.ByLayerBottomFirst.Should().HaveCount(3);
    }

    [Fact]
    public async Task When_A_Component_Is_Added_As_A_Child_Of_A_NodeThat_Is_Not_Yet_Add_Then_Should_Not_Be_Attached()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);
        
        TvComponent root = TvComponent.CreateStatelessComponent();
        TvComponent child = TvComponent.CreateStatelessComponent();
        var childNode = await ui.ComponentTree.AddChild(child, root);
        
        childNode.Metadata.IsAttached.Should().BeFalse();
    }
    
    [Fact]
    public async Task When_A_Component_Is_Added_As_Root_All_Its_Childs_Should_Be_Attached()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);
        
        TvComponent root = TvComponent.CreateStatelessComponent();
        TvComponent child = TvComponent.CreateStatelessComponent();
        var childNode = await ui.ComponentTree.AddChild(child, root);
        var rootNode = await ui.ComponentTree.Add(root);

        rootNode.Metadata.IsAttached.Should().BeTrue();
        childNode.Metadata.IsAttached.Should().BeTrue();
    }

    [Fact]
    public async Task When_A_Component_Is_Added_As_A_Child_Of_Itseld_Then_Should_Be_An_Error()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);
        
        TvComponent root = TvComponent.CreateStatelessComponent();
        var child = root;
        async Task DoAddChild() => await ui.ComponentTree.AddChild(child, root);
        await Assert.ThrowsAsync<ArgumentException>(DoAddChild);
    }
    
}