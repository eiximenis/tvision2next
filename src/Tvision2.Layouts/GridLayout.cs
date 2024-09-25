using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Engine.Components;
using Tvision2.Engine.Layouts;

namespace Tvision2.Layouts
{
    class GridLayout : ILayoutManager
    {
        private readonly TvComponentMetadata _container;
        public bool HasLayoutPending { get; private set; }
        public GridLayout(TvComponentMetadata container )
        {
            _container = container;
            _container.On().ViewportUpdated.Do(OnContainerUpdated);
        }
        private void OnContainerUpdated(ViewportUpdateReason reason)
        {
            HasLayoutPending = true;
        }

        public ViewportUpdateReason UpdateLayout(TvComponentMetadata metadata)
        {
            
        }
    }
}
