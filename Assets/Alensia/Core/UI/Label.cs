using UnityEngine;

namespace Alensia.Core.UI
{
    public class Label : UIComponent, IContentHolder
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

        public override Vector2 MinimumSize => this.MinimumSize(Style);
        
        public override Vector2 PreferredSize => this.PreferredSize(Style);

        protected override GUIStyle DefaultStyle => Manager.Skin.label;

        public Label(IUIManager manager) : base(manager)
        {
        }

        protected override void PaintComponent()
        {
            GUI.Label(Bounds, Content, Style);
        }
    }
}