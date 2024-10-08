namespace Tvision2.Drawing.Tables;

class TableCell
{
    private IBoundedElement? _content;

    public void SetContent(string value) => SetContent(new StringBoundedElement(value));

    public void SetContent(IBoundedElement value) 
    {
        _content = value;
    }
}