using System.Collections.Generic;
using Alensia.Core.Common;

namespace Alensia.Core.Collection
{
    public abstract class Directory<T> : IDirectory<T>, IEditorSettings
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
    }
}