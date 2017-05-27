using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public abstract class Layout<T> : ILayout
    {
        public IReadOnlyList<IComponent> Components => _children;

        private readonly List<IComponent> _children = new List<IComponent>();

        private readonly IDictionary<IComponent, T> _constraints = new Dictionary<IComponent, T>();

        public bool Contains(IComponent child)
        {
            lock (this)
            {
                return _constraints.ContainsKey(child);
            }
        }

        public void Add(IComponent child)
        {
            Assert.IsNotNull(child, "child != null");

            lock (this)
            {
                if (_children.Contains(child)) return;

                var constraints = child.LayoutConstraints;

                Assert.IsTrue(constraints == null || constraints is T, "constraints is T");

                _children.Add(child);
                _constraints.Add(child, (T) constraints);
            }
        }

        public void Remove(IComponent child)
        {
            Assert.IsNotNull(child, "child != null");

            lock (this)
            {
                _children.Remove(child);
                _constraints.Remove(child);
            }
        }

        public virtual void RemoveAll()
        {
            lock (this)
            {
                _children.Clear();
                _constraints.Clear();
            }
        }

        public abstract Vector2 CalculateMinimumSize(IContainer container);

        public abstract Vector2 CalculatePreferredSize(IContainer container);

        public abstract void PerformLayout(IContainer container);
    }
}