using Tvision2.Engine.Components;
using Tvision2.Messaging.Extensions;

namespace Tvision2.Messaging;

public class TvMessageBus
{
    public const string TVMESSAGEINVOKERTAG = nameof(TvMessageInvoker);
    public TvMessageBus(ITvComponentTree tree)
    { 
    }

    public void EnqueueMessageTo(TvComponent component, TvMessage message)
    {
        if (!component.Metadata.IsAttached)
        {
            return;
        }

        var tree = component.Metadata.OwnerTree!;
        var node = component.Metadata.Node;
        var invoker = component.Metadata.GetOrCreateTag(TVMESSAGEINVOKERTAG,() => new TvMessageInvoker(node));
        invoker.Enqueue(message);
    }
}

public class TvMessage
{
    public string Name { get; }
    public TvMessage(string name) => Name = name;
}

public class TvMessage<TM> : TvMessage
{
    public TM Data { get; }

    public TvMessage(string name, TM data) : base(name)
    {
        Data = data;
    }
}

public class TvMessageInvoker
{
    private readonly List<TvMessage> _messages = [];
    private readonly TvComponentTreeNode _node;
    
    public TvMessageInvoker(TvComponentTreeNode node)
    {
        _node = node;

        
    }

    public void Enqueue(TvMessage message)
    {
        _messages.Add(message);
    }

    public bool HasQuededMessages => _messages.Count > 0;

    public async Task ProcessMessages()
    {
        var handlers = _node.Metadata.GetTag<MessageHandlersOptions>(TvComponentExtensions_Messaging.MESSAGES_HANDLERS_TAG);
        if (handlers is null)
        {
            return;
        }
        foreach (var message in _messages)
        {
            handlers.Invoke(message);

        }
        _messages.Clear();
    }



}