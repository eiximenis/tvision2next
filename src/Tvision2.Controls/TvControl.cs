using Tvision2.Engine.Components;

namespace Tvision2.Controls;

public abstract class TvControl
{
    internal const string CONTROL_TAG = "Tvision2::Control";

    public static void Wrap<TState, TOptions>(TvComponent<TState> componentToWrap, TOptions options)
    {
        componentToWrap.Metadata.TagWith(CONTROL_TAG, new TvControlMetadata());
    }
}