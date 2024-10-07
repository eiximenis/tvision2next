using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Tvision2.Drawing.Text;
using Wcwidth;

namespace Tvision2.Console;

partial class TvConsole
{
    public static void Tabulate(TabulatedText text)
    {
        var lines = text.GetLines();

        foreach (var line in lines)
        {
            Write(line.Runes);
            WriteLine();
        }

    }

}
