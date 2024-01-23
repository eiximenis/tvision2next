using Tvision2.Engine.Components;

namespace Tvision2.Messaging;

class MessageProcBehavior<T> : ITvBehavior<T>
{

    private readonly Func<Task> _messsageProc;
    public MessageProcBehavior(Func<Task> messageProc)
    {
        _messsageProc = messageProc;
    }
    
    public void Do(BehaviorContext<T> context)
    {
    }
}