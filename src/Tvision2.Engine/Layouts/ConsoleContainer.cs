using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Console.Events;
using Tvision2.Core;
using Tvision2.Engine.Components;
using Tvision2.Engine.Events;

namespace Tvision2.Engine.Layouts
{
    public class ConsoleContainer : ITvContainer, ITvContainerActions, IHook
    {
        private readonly ActionsChain<ViewportUpdateReason> _viewportUpdated = new();
        private ViewportSnapshot _viewport;

        public ConsoleContainer(Tvision2Engine engine)
        {
            engine.UI.AddHook(this);
        }

        public IViewportSnapshot Viewport => _viewport;

        IActionsChain<ViewportUpdateReason> ITvContainerActions.ViewportUpdated => _viewportUpdated;
        public ITvContainerActions On() => this;

        async Task IHook.BeforeUpdate(TvConsoleEvents events)
        {
            if (events.HasWindowEvent)
            {
                var windowEvent = events.WindowEvent!;
                _viewport = new ViewportSnapshot(TvPoint.Zero, TvBounds.FromRowsAndCols(windowEvent.NewRows, windowEvent.NewColumns));
                await _viewportUpdated.Invoke(ViewportUpdateReason.Resized);
            }
        }
    }
}
