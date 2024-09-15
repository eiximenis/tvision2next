using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Tvision2.Core;
using Tvision2.Drawing.Shapes;
using Tvision2.Drawing.Text;
using Wcwidth;

namespace Tvision2.Console;

partial class TvConsole
{
    public static void Wrap<TShape>(string text, TShape shape, Justification justification = Justification.None) where TShape : IShape
    {
        WrappedText.Draw(ConsoleDrawer, text, shape, TvColorsPair.FromForegroundAndBackground(Foreground, Background), justification);
    }
};