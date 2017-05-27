using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Alensia.Core.UI
{
    public abstract class UIContainer : UIComponent, IContainer
    {
        public ILayout Layout { get; }

        public IReadOnlyList<IComponent> Components => Layout.Components;

        public override Vector2 MinimumSize => Layout.CalculateMinimumSize(this);

        public override Vector2 PreferredSize => Layout.CalculatePreferredSize(this);

        public virtual RectOffset InnerPadding => new RectOffset(0, 0, 0, 0);

        private bool _hasValidLayout;

        protected UIContainer(IUIManager manager) : this(new NullLayout(), manager)
        {
        }

        protected UIContainer(ILayout layout, IUIManager manager) : this(layout, null, manager)
        {
        }

        protected UIContainer(ILayout layout, object constraints, IUIManager manager) :
            base(constraints, manager)
        {
            Assert.IsNotNull(layout, "layout != null");

            layout.RemoveAll();

            Layout = layout;
        }

        public bool Contains(IComponent child) => Layout.Contains(child);

        public virtual void Add(IComponent child)
        {
            lock (this)
            {
                Layout.Add(child);

                child.Parent = this;

                InvalidateLayout();
            }
        }

        public virtual void Remove(IComponent child)
        {
            lock (this)
            {
                Layout.Remove(child);

                child.Parent = null;

                InvalidateLayout();
            }
        }

        public virtual void RemoveAll()
        {
            lock (this)
            {
                var children = new List<IComponent>(Components);

                Layout.RemoveAll();

                children.ForEach(c => c.Parent = null);

                InvalidateLayout();
            }
        }

        public void Pack() => Size = PreferredSize;

        public virtual void InvalidateLayout() => _hasValidLayout = false;

        protected virtual void PerformLayout()
        {
            if (!Visible) return;

            lock (this)
            {
                Layout.PerformLayout(this);

                _hasValidLayout = true;
            }
        }

        protected override void PaintComponent()
        {
            if (!_hasValidLayout) PerformLayout();

            PaintChildren();
        }

        protected virtual void PaintChildren() => Components.ToList().ForEach(PaintChild);

        protected virtual void PaintChild(IComponent child)
        {
            Assert.IsNotNull(child, "child != null");

            child.Paint();
        }
    }
}