using System.Collections.Generic;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public class ComponentsHolder : IComponentsHolder
    {
        public IReadOnlyList<IComponent> Components => _children;

        public IObservable<IComponent> ComponentAdded => _componentAdded;

        public IObservable<IComponent> ComponentRemoved => _componentRemoved;

        private readonly List<IComponent> _children = new List<IComponent>();

        private readonly Subject<IComponent> _componentAdded = new Subject<IComponent>();

        private readonly Subject<IComponent> _componentRemoved = new Subject<IComponent>();

        public bool Contains(IComponent child)
        {
            lock (this)
            {
                return _children.Contains(child);
            }
        }

        public void Add(IComponent child)
        {
            Assert.IsNotNull(child, "child != null");

            lock (this)
            {
                if (_children.Contains(child)) return;

                _children.Add(child);
            }

            _componentAdded.OnNext(child);
        }

        public void Remove(IComponent child)
        {
            Assert.IsNotNull(child, "child != null");

            lock (this)
            {
                if (!_children.Contains(child)) return;

                _children.Remove(child);
            }

            _componentRemoved.OnNext(child);
        }

        public virtual void RemoveAll()
        {
            lock (this)
            {
                _children.Clear();
            }
        }
    }
}