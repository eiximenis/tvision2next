using Tvision2.Engine.Components;

namespace Tvision2.Messaging.Extensions;

public static class TvComponentExtensions_Messaging
{
    public static void AddMessageHandler<T>(this TvComponent<T> component, Func<Task> messageProc)
    {
        component.AddBehavior(new MessageProcBehavior<T>(messageProc));
    }
    
}