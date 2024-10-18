namespace Tvision2.Drawing.Tables;

public enum ColumnWidthType
{
    Fixed,
    Relative
}
public readonly record struct ColumnWidth(ColumnWidthType Type, int Value)
{
    public static ColumnWidth Fixed(int value) => new ColumnWidth(ColumnWidthType.Fixed, value);
    public static ColumnWidth Relative(int value) => new ColumnWidth(ColumnWidthType.Relative, value);
}