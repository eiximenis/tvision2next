using Tvision2.Engine.Components;

namespace Tvision2.Controls.Extensions;

public static class TvComponentExtensions_Controls
{

    public static bool IsControl(this TvComponent cmp) => cmp.Metadata.IsControl();
    public static bool IsControl(this TvComponentMetadata metadata) => metadata.HasTag(TvControl.CONTROL_TAG);

    public static TvControlMetadata? GetControlMetadata(this TvComponentMetadata metadata) =>
        metadata.GetTag<TvControlMetadata>(TvControl.CONTROL_TAG);
}