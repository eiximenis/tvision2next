namespace Tvision2.Console.Events;

public sealed class TvWindowEvent 
{
    public int NewColumns { get; private set; }
    public int NewRows { get; private set; }

    public bool IsHandled { get; private set; }


    public TvWindowEvent(int cols, int rows)
    {
        Update(cols, rows); 
    }

    internal void Update(int cols, int rows)
    {
        NewColumns = cols;
        NewRows = rows;
        IsHandled = false;
    }
}