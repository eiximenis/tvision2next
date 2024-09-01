using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Engine.Events;

namespace Tvision2.Controls.Windows;

public interface IWindowActions : ITvEventedControlActions
{

}

public class TvWindow : TvEventedControl<Unit>, IWindowActions
{
	public TvWindow() : base (Unit.Value)
    {
        Options.AutoSize = false;
        Options.FocusPolicy = FocusPolicy.DirectFocusable;
    }


}