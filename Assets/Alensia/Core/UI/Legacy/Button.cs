using UniRx;
using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    public class Button : UIComponent, IContentHolder, IClickable<Button>
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

        public IObservable<Button> Clicked { get; } = new Subject<Button>();

        protected override GUIStyle DefaultStyle => Manager.Skin.button;

        public Button(IUIManager manager) : this(null, manager)
        {
        }

        public Button(object constraints, IUIManager manager) : base(constraints, manager)
        {
        }

        protected override void PaintComponent()
        {
            if (GUI.Button(Bounds, Content, Style))
            {
                ((Subject<Button>) Clicked).OnNext(this);
            }
        }
    }
}