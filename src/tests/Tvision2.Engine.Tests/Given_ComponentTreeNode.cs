using Microsoft.Extensions.Configuration;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Tests;

[Trait("Category", "Components Tree")]
public class Given_ComponentTreeNode
{
    [Fact]
    public async Task With_Childs_Then_Its_Childs_Should_Be_Returned_As_Descendants()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);
        var root = TvComponent.CreateStatelessComponent();
        var child = TvComponent.CreateStatelessComponent();
        var child2 = TvComponent.CreateStatelessComponent();

        ui.ComponentTree.AddChild(child, root);
        ui.ComponentTree.AddChild(child2, root);

        var rootNode = root.Metadata.Node;
        rootNode.Descendants().Should().HaveCount(2);
    }

    [Fact]
    public async Task Without_Childs_Then_Its_Descendants_Should_Be_Empty()
    {
        var root = TvComponent.CreateStatelessComponent();
        root.Metadata.Node.Descendants().Should().BeEmpty();
    }

    [Fact]
    public async Task Without_Childs_Then_Its_SubTree_Should_Contains_Only_Itself()
    {
        var root = TvComponent.CreateStatelessComponent();
        var node = root.Metadata.Node;
        node.SubTree().Should().ContainSingle(item => item == node);
    }
}