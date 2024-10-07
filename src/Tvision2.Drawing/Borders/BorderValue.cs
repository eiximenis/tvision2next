namespace Tvision2.Drawing.Borders;

public enum BorderType
{
    None = 0,
    Spaces = 1,
    Fill = 2,
    Double = 3,
    Single = 4
}
public readonly struct BorderValue
{
    private readonly int _value;

    public BorderValue(BorderType horizontal, BorderType vertical)
    {
        _value = ((int)horizontal | (int)vertical << 4);
    }

    public BorderValue(BorderType border) : this(border, border) { }

    public static BorderValue None() => new BorderValue(BorderType.None, BorderType.None);
    public static BorderValue Double() => new BorderValue(BorderType.Double, BorderType.Double);
    public static BorderValue Single() => new BorderValue(BorderType.Single, BorderType.Single);
    public static BorderValue Spaces() => new BorderValue(BorderType.Spaces, BorderType.Spaces);

    public static BorderValue HorizontalVertical(BorderType horizontal, BorderType vertical) => new BorderValue(horizontal, vertical);

    public bool HasVerticalBorder => (_value & 0b11110000) != 0;
    public bool HasHorizontalBorder => (_value & 0b00001111) != 0;

    public bool HasBorder => _value != 0;


    public void Deconstruct(out BorderType horizontal, out BorderType vertical)
    {
        horizontal = Horizontal;
        vertical = Vertical;
    }


    public BorderType Horizontal => (BorderType)((_value & 0b11110000) >> 4);

    public BorderType Vertical => (BorderType)(_value & 0b00001111);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is BorderValue bv)
        {
            return bv._value == _value;
        }

        return base.Equals(obj);
    }

    public static bool operator ==(BorderValue left, BorderValue right) => left._value == right._value;
    public static bool operator !=(BorderValue left, BorderValue right) => left._value != right._value;

}