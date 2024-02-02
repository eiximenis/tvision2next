using Tvision2.Core;
using Tvision2.Engine;
using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public class TvControlsTree
{
    private readonly ITvComponentTree _componentTree;
    
    public TvControlsTree(Tvision2Engine engine)
    {
        _componentTree = engine.UI.ComponentTree;

        _componentTree.On().TreeUpdated.Do(OnComponentTreeUpdated);
    }

    private void OnComponentTreeUpdated(Unit _)
    {
  
    }
}