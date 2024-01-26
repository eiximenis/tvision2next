namespace Tvision2.Console.Input;

public class SequenceNode
{
    public char Token { get; }
    public bool EndNode { get; }
        
    public string FullSequence { get; }
        
        
    public ConsoleKeyInfo KeyInfo { get; }
        
        
    public SortedList<char, SequenceNode> Childs { get;}

    public  SequenceNode(char token,  string fullSequence, bool leaf, ConsoleKeyInfo keyInfo)
    {
        Token = token;
        EndNode = leaf;
        FullSequence = fullSequence;
        KeyInfo = keyInfo;
        if (!EndNode)
        {
            Childs = new SortedList<char, SequenceNode>();
        }
    }
}

public interface ISequenceReader

{
    void AddSequences(IEnumerable<InputSequence> sequences);
    void Start();       // Starts a new sequence
    void Push(in int nextKey);     // Push key into current sequence
    SequenceNode? CheckSequence();  // Check if current key sequence is valid
    IEnumerable<SequenceNode> CheckSequences(); // Check all pending sequences
    
}