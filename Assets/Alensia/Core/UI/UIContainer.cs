using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public abstract class UIContainer : UIComponent, IContainer
    {
        public ILayout Layout { get; }

        public IReadOnlyList<IComponent> Components => _components.Components;

        public override Vector2 MinimumSize => Layout.CalculateMinimumSize(this);

        public override Vector2 PreferredSize => Layout.CalculatePreferredSize(this);

        public virtual RectOffset InnerPadding => new RectOffset(0, 0, 0, 0);

        public IObservable<IComponent> ComponentAdded => _components.ComponentAdded;

        public IObservable<IComponent> ComponentRemoved => _components.ComponentRemoved;

        private readonly IComponentsHolder _components = new ComponentsHolder();

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

            _components.RemoveAll();

            Layout = layout;
        }

        public bool Contains(IComponent child) => _components.Contains(child);

        public virtual void Add(IComponent child)
        {
            lock (this)
            {
                _components.Add(child);

                child.Parent = this;

                InvalidateLayout();
            }
        }

        public virtual void Remove(IComponent child)
        {
            lock (this)
            {
                _components.Remove(child);

                child.Parent = null;

                InvalidateLayout();
            }
        }

        public virtual void RemoveAll()
        {
            lock (this)
            {
                var children = new List<IComponent>(Components);

                _components.RemoveAll();

                children.ForEach(c => c.Parent = null);

                InvalidateLayout();
            }
        }

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