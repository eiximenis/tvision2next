using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;

namespace Tvision2.Layouts
{
    class ComponentContainer : ITvContainer, ITvContainerActions
    {
        private readonly TvComponentMetadata _componentMetadata;
        private readonly Margin _margin;

        public ComponentContainer(TvComponent owner, Margin margin)
        {
            _componentMetadata = owner.Metadata;
            _margin = margin;
        }

        public ITvContainerActions On() => this;

        public IViewportSnapshot Viewport => _margin switch
        {
            { IsNone: true } => _componentMetadata.Component.Viewport,
            _ => ViewportSnapshots.WithMargin(_componentMetadata.Component.Viewport, _margin)
        };
        public IActionsChain<ViewportUpdateReason> ViewportUpdated => _componentMetadata.On().ViewportUpdated;
    }

    public static class ComponentContainerExtensions
    {
        public static ITvContainer AsContainer(this TvComponent component)
        {
            return new ComponentContainer(component, Margin.None);
        }

        public static ITvContainer AsContainer(this TvComponent component, Margin margin)
        {
            return new ComponentContainer(component, margin);
        }

    }
}
