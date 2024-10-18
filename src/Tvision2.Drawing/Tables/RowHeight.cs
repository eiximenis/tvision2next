using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Drawing.Tables;

public enum RowHeightType
{
    Fixed,
    Relative
}

public readonly record struct RowHeight(RowHeightType Type, int Value)
{
    public static RowHeight Fixed(int value) => new RowHeight(RowHeightType.Fixed, value);
    public static RowHeight Relative(int value) => new RowHeight(RowHeightType.Relative, value);
}
