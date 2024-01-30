using Tvision2.Core;
using Tvision2.Engine.Components;

namespace Tvision2.Controls.Extensions;

public static class TvControlFactoryExtensions_Controls
{
    public static TvLabel CreateLabel(this IControlFactory factory, string text, Action<TvLabelOptions>? optionsAction = null, Viewport? viewport = null)
    {
        viewport ??= new Viewport(TvPoint.Zero, TvBounds.FromRowsAndCols(1, text.Length));
       var component = TvComponent.Create(text,  viewport); 
       var options = new TvLabelOptions();
       optionsAction?.Invoke(options);
       return new TvLabel(component, optionsAction);
    }
    
    public static TvLabel CreateLabel(this IControlFactory factory, TvComponent<string> component, Action<TvLabelOptions>? optionsAction = null)
    {
        var options = new TvLabelOptions();
        optionsAction?.Invoke(options);
        return new TvLabel(component, optionsAction);
    }
    
}