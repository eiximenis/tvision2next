using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Engine.Components;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts;
class ContainerLayoutManager : ILayoutManager
{
    private readonly TvComponentMetadata _container;

    public bool HasLayoutPending { get; private set; }

    public ContainerLayoutManager(TvContainer container)
    {
        _container = container.ComponentMetadata;

    }
    public ViewportUpdateReason UpdateLayout(TvComponentMetadata metadata)
    {
        throw new NotImplementedException();
    }
}
