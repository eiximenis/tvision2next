using Tvision2.Engine.Components;

namespace Tvision2.Engine.Layouts;

public interface ILayoutManager
{
     
     /// <summary>
     /// True if this layout needs to be recalculated (UpdateLayout called).
     /// </summary>
     bool HasLayoutPending => false;
     
     /// <summary>
     /// Layout for component specified by the metadata param
     /// must be updated. This method can safely update the viewport.
     /// This method is called if the layout (using HasLayoutPending) OR
     /// the Viewport says that layout needs to be recalculated.
     /// </summary>  
     ViewportUpdateReason UpdateLayout(Viewport viewportToUpdate);

     /// <summary>
     /// The layout is being dismissed. That is useful to unsubscribe to any event or perform cleaning.
     /// </summary>
     void Dismiss()
     {
     }
}