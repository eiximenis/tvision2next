using Tvision2.Engine.Components;

namespace Tvision2.Controls.Extensions;

public static class TvComponentExtensions_Controls
{
    public static TvControl<TState, TOptions> Wrap<TState, TOptions>(this TvComponent<TState> componentToWrap, TOptions options)
    {
        return new TvControl<TState, TOptions>(componentToWrap, options);
    }

    public static bool IsControl(this TvComponent cmp) => cmp.Metadata.IsControl();
    public static bool IsControl(this TvComponentMetadata metadata) => metadata.HasTag(TvControl.CONTROL_TAG);
}