using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Engine;
using Tvision2.Engine.Components;

namespace Tvision2.Messaging.Extensions;


public class MessageHandlersOptions
{
    private readonly Dictionary<string, Delegate> _handlers = new ();

    public void AddMessageHandler<TM>(Func<TvMessage<TM>, Task> message)
    {
        var key = nameof(TM);
        _handlers[key] = message;
    }

    public void Invoke(TvMessage message)
    {
        var key = message.Name;
        var handler = _handlers.GetValueOrDefault(key);
        handler?.DynamicInvoke(message);
    }
}

public static class TvComponentExtensions_Messaging
{
    internal const string MESSAGES_HANDLERS_TAG="Tvision2.Messaging";
    public static void AddMessageHandler<T, TM>(this TvComponent<T> component, Func<TvMessage<TM>, Task> messageProc)
    {
        var options = component.Metadata.GetOrCreateTag(MESSAGES_HANDLERS_TAG, () => new MessageHandlersOptions());
        options.AddMessageHandler(messageProc);
    }
}