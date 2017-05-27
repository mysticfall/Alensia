using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public abstract class UIComponent : IComponent
    {
        public IUIManager Manager { get; }

        public IContainer Parent { get; set; }

        public bool Active { get; set; } = true;

        public bool Visible { get; set; } = true;

        public Color? Color { get; set; }

        public Color? BackgroundColor { get; set; }

        public Rect Bounds
        {
            get { return _bounds; }
            set
            {
                if (value == _bounds) return;

                var min = MinimumSize;

                var width = Mathf.Max(min.x, value.width);
                var height = Mathf.Max(min.y, value.height);

                value.size = new Vector2(width, height);

                _bounds = value;

                Parent?.InvalidateLayout();
            }
        }

        public Vector2 Position
        {
            get { return _bounds.position; }
            set
            {
                _bounds.position = value;

                Parent?.InvalidateLayout();
            }
        }

        public Vector2 Size
        {
            get { return _bounds.size; }
            set
            {
                _bounds.size = value;

                Parent?.InvalidateLayout();
            }
        }

        public RectOffset Padding
        {
            get { return _padding ?? Style.padding; }
            set
            {
                _padding = value;

                InvalidateStyle();

                Parent?.InvalidateLayout();
            }
        }

        public RectOffset Margin
        {
            get { return _margin ?? Style.margin; }
            set
            {
                _margin = value;

                InvalidateStyle();

                Parent?.InvalidateLayout();
            }
        }

        public GUIStyle Style
        {
            get
            {
                lock (this)
                {
                    if (_effectiveStyle != null) return _effectiveStyle;

                    _effectiveStyle = CreateStyle(new GUIStyle(_style ?? DefaultStyle));

                    return _effectiveStyle;
                }
            }
            set
            {
                _style = value;

                InvalidateStyle();

                Parent?.InvalidateLayout();
            }
        }

        public abstract Vector2 MinimumSize { get; }

        public abstract Vector2 PreferredSize { get; }

        public object LayoutConstraints { get; set; }

        protected abstract GUIStyle DefaultStyle { get; }

        private Rect _bounds;

        private RectOffset _padding;

        private RectOffset _margin;

        private GUIStyle _style;

        private GUIStyle _effectiveStyle;

        private Action _painJob;

        protected UIComponent(IUIManager manager) : this(null, manager)
        {
        }

        protected UIComponent(object constraints, IUIManager manager)
        {
            Assert.IsNotNull(manager, "manager != null");

            Manager = manager;
            LayoutConstraints = constraints;

            _painJob = Initialize;
        }

        protected virtual void Initialize()
        {
        }

        protected virtual GUIStyle CreateStyle(GUIStyle style)
        {
            if (_margin != null) style.margin = _margin;
            if (_padding != null) style.padding = _padding;

            return style;
        }

        public virtual void InvalidateStyle()
        {
            lock (this)
            {
                _effectiveStyle = null;
            }
        }

        protected virtual void RunWithStyle(Action action)
        {
            var wasActive = GUI.enabled;

            Color? previousColor = null;
            Color? previousBackground = null;

            GUI.enabled = Active;

            if (Color.HasValue)
            {
                previousColor = GUI.color;

                GUI.color = Color.Value;
            }

            if (BackgroundColor.HasValue)
            {
                previousBackground = GUI.backgroundColor;

                GUI.backgroundColor = BackgroundColor.Value;
            }

            action.Invoke();

            GUI.enabled = wasActive;

            if (previousColor.HasValue)
            {
                GUI.color = previousColor.Value;
            }

            if (previousBackground.HasValue)
            {
                GUI.backgroundColor = previousBackground.Value;
            }
        }


        public virtual void Paint()
        {
            lock (this)
            {
                _painJob?.Invoke();
                _painJob = null;
            }

            if (!Visible) return;

            RunWithStyle(PaintComponent);
        }

        public void OnNextPaint(Action action) => _painJob = action;

        protected abstract void PaintComponent();
    }
}