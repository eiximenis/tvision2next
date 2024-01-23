namespace Tvision2.Console;

public class InputSequence
{
    public string Sequence { get; }
    public ConsoleKeyInfo KeyInfo { get; }

    public InputSequence(string seq, ConsoleKeyInfo keyInfo)
    {
        Sequence = seq;
        KeyInfo = keyInfo;
    }
}