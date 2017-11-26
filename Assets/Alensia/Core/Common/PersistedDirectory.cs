using System.Collections.Generic;
using UnityEngine;

namespace Alensia.Core.Common
{
    public abstract class PersistedDirectory<T> : ScriptableObject, IDirectory<T>
        where T : class, INamed
    {
        protected abstract IEnumerable<T> Items { get; }

        protected IDictionary<string, T> ItemMap
        {
            get
            {
                lock (this)
                {
                    if (_itemMap != null) return _itemMap;

                    _itemMap = new Dictionary<string, T>();

                    foreach (var item in Items)
                    {
                        _itemMap.Add(item.Name, item);
                    }

                    return _itemMap;
                }
            }
        }

        private IDictionary<string, T> _itemMap;

        public bool Contains(string key) => ItemMap.ContainsKey(key);

        public T this[string key] => ItemMap.ContainsKey(key) ? ItemMap[key] : null;

        private void OnValidate() => _itemMap = null;

        private void OnDestroy() => _itemMap = null;
    }
}