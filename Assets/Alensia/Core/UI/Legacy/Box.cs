using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public class Box : UIContainer, IContentHolder
    {
        public string Text
        {
            get { return Content.text; }
            set { Content.text = value; }
        }

        public Texture Image
        {
            get { return Content.image; }
            set { Content.image = value; }
        }

        public string Tooltip
        {
            get { return Content.tooltip; }
            set { Content.tooltip = value; }
        }

        public GUIContent Content { get; } = new GUIContent();

        public override Vector2 MinimumSize => Layout.CalculateMinimumSize(this);

        public override Vector2 PreferredSize => Layout.CalculatePreferredSize(this);

        public override RectOffset InnerPadding
        {
            get
            {
                var header = Style.CalcSize(Content);

                var padding = base.InnerPadding;

                padding.top += (int) header.y;

                return padding;
            }
        }

        protected override GUIStyle DefaultStyle => Manager.Skin.box;

        public Box(IUIManager manager) : base(manager)
        {
        }

        public Box(ILayout layout, IUIManager manager) : base(layout, manager)
        {
        }

        protected override void PaintChildren()
        {
            GUI.Box(Bounds, Content, Style);

            base.PaintChildren();
        }
    }
}