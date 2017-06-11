using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public class Group : UIContainer, ICoordinatesHost
    {
        protected override GUIStyle DefaultStyle => GUIStyle.none;

        public Group(IUIManager manager) : base(manager)
        {
        }

        public Group(ILayout layout, IUIManager manager) : base(layout, manager)
        {
        }

        protected override void PaintChildren()
        {
            GUI.BeginGroup(Bounds, Style);

            base.PaintChildren();

            GUI.EndGroup();
        }
    }
}