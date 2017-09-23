using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Common
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> GetChildren(this Transform parent) => new ChildEnumerable(parent);

        public static IEnumerable<T> GetChildren<T>(this Transform parent)
        {
            return new ChildEnumerable(parent)
                .ToList()
                .Select(c => c.GetComponent<T>())
                .Where(c => c != null);
        }

        private class ChildEnumerable : IEnumerable<Transform>
        {
            private readonly Transform _parent;

            public ChildEnumerable(Transform parent)
            {
                Assert.IsNotNull(parent, "parent != null");

                _parent = parent;
            }

            public IEnumerator<Transform> GetEnumerator() => new ChildEnumerator(_parent);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private class ChildEnumerator : IEnumerator<Transform>
            {
                public Transform Current { get; private set; }

                object IEnumerator.Current => Current;

                private readonly Transform _parent;

                private int _count;

                private int _index;

                public ChildEnumerator(Transform parent)
                {
                    _parent = parent;

                    Reset();
                }

                public bool MoveNext()
                {
                    lock (this)
                    {
                        Current = _index < _count ? _parent.GetChild(_index++) : null;

                        return Current != null;
                    }
                }

                public void Reset()
                {
                    lock (this)
                    {
                        Current = null;

                        _count = _parent.childCount;
                        _index = 0;
                    }
                }

                public void Dispose()
                {
                }
            }
        }
    }
}