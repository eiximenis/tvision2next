using Tvision2.Engine.Components;

namespace Tvision2.Styles.Extensions
{
    public interface IStyledDrawer<T>
    {
        DrawResult Draw(in StyledConsoleContext context, T data);
        ITvDrawer<T> ToStandardDrawer();
    }
}
