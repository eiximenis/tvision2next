using Tvision2.Engine.Components;

namespace Tvision2.Messaging.Extensions;


public class MessageOptions
{
}

public static class TvComponentExtensions_Messaging
{
    private const string MESSAGE_TAG_LABEL="Tvision2.Messaging";
    public static void AddMessageHandler<T>(this TvComponent<T> component, Func<Task> messageProc)
    {
        component.Metadata.TagWith(MESSAGE_TAG_LABEL, new MessageOptions());
        component.AddBehavior(new MessageProcBehavior<T>(messageProc));
    }
}