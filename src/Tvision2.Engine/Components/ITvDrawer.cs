using Tvision2.Core.Engine.Render;

namespace Tvision2.Engine.Components;

public interface ITvDrawer<T>
{
    void Draw(in ConsoleContext context, T data);
}
