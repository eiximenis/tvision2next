using Tvision2.Engine.Components;

namespace Tvision2.Messaging;

class MessageProcBehavior<T> : ITvBehavior<T>
{

    private readonly Func<Task> _messsageProc;
    private readonly MessageBus _bus;
    public MessageProcBehavior(Func<Task> messageProc, MessageBus bus)
    {
        _messsageProc = messageProc;
    }
    
    public void Do(BehaviorContext<T> context)
    {
        
    }
    
}