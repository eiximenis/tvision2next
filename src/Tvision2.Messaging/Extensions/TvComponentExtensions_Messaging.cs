using Microsoft.Extensions.DependencyInjection;
using Tvision2.Engine;
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
        if (component.Metadata.HasTag(MESSAGE_TAG_LABEL))
        {
            throw new InvalidOperationException("Component already has Messaging enabled!");
        }
        
        component.Metadata.TagWith(MESSAGE_TAG_LABEL, new MessageOptions());
        var messageBus = Tv2App.GetEngine().GetRegisteredComponent<MessageBus>();
        var behavior = new MessageProcBehavior<T>(messageProc, messageBus);
        component.AddBehavior(behavior);
    }
}