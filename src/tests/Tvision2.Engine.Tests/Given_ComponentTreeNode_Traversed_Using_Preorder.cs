using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Render;

namespace Tvision2.Engine.Tests;

[Trait("Category", "Components Tree")]
public class Given_ComponentTreeNode_Traversed_Using_Preorder
{
    [Fact]
    public void Without_Childs_Then_Should_Contains_Only_Itself()
    {
        var root = TvComponent.CreateStatelessComponent();
        var node = root.Metadata.Node;
        node.PreOrder.Should().ContainSingle(item => item == node);
    }

    [Fact]
    public async Task With_Single_Level_Of_Child_Then_Root_Should_Be_Returned_First()
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
        rootNode.PreOrder.Should().HaveCount(3);
        rootNode.PreOrder.Should().StartWith(rootNode);
    }

    [Fact]
    public async Task With_Multiple_Levels_Of_Childs_Then_Superior_Levels_Should_Be_Returned_First()
    {
        var bounds = TvBounds.FromRowsAndCols(24, 80);
        var driver = new InMemoryConsoleDriver(bounds);
        var console = new VirtualConsole(bounds, TvColor.Black );
        var ui = new TvUiManager(console, driver);
        
        // Build a Tree
        //
        //               Root
        //                |\
        //               A  B
        //              /\   \
        //            AA AB  BA
        //               |
        //              ABA
        var root = TvComponent.Create("ROOT");
        var nodeA = TvComponent.Create("A");
        var nodeB = TvComponent.Create("B");
        ui.ComponentTree.AddChild(nodeA, root);
        ui.ComponentTree.AddChild(nodeB, root);

        var nodeAA = TvComponent.Create("AA");
        var nodeAB = TvComponent.Create("AB");
        var nodeBA = TvComponent.Create("BA");
        ui.ComponentTree.AddChild(nodeAA, nodeA);
        ui.ComponentTree.AddChild(nodeAB, nodeA);
        ui.ComponentTree.AddChild(nodeBA, nodeB);

        var nodeABA = TvComponent.Create("ABA");
        ui.ComponentTree.AddChild(nodeABA, nodeAB);
        // PreOrder is: root, A, AA, AB, ABA, B, BA

        TvComponentTreeNode[] expected = [root.Metadata.Node, nodeA.Metadata.Node, nodeAA.Metadata.Node, nodeAB.Metadata.Node, nodeABA.Metadata.Node, nodeB.Metadata.Node, nodeBA.Metadata.Node];
        var preorder = root.Metadata.Node.PreOrder;
        preorder.Should().ContainInConsecutiveOrder(expected);
    }
}