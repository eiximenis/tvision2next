using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Engine.Components;

namespace Tvision2.Styles.Extensions
{

    class FuncStyledDrawer<T> : StyledDrawer<T>
    {
        private readonly Func<StyledConsoleContext, T, DrawResult> _drawerFunc;
        public FuncStyledDrawer(TvComponent<T> component, Func<StyledConsoleContext, T, DrawResult> drawerFunc, string? styleSetName) : base(component, styleSetName)
        {
            _drawerFunc = drawerFunc;
        }

        public override DrawResult Draw(in StyledConsoleContext context, T data)
        {
            return _drawerFunc(context, data);
        }
    }





    public static class TvComponentExtensions_Styles
    {
        public static void AddStyledDrawer<T>(this TvComponent<T> component, IStyledDrawer<T> drawer)
        {
            component.AddDrawer(drawer.ToStandardDrawer());

        }

        public static void AddStyledDrawer<T>(this TvComponent<T> component, Func<StyledConsoleContext, T, DrawResult> drawerFunc, string? styleSetName = null)
        {
            var drawer = new FuncStyledDrawer<T>(component, drawerFunc, styleSetName);
            component.AddDrawer(drawer);
        }
    }
}
