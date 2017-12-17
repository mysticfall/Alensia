using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Collection;
using Alensia.Core.Common;
using Malee;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Item
{
    public abstract class SlotContainer<TSlot, TItem, TImpl, TList> : ManagedMonoBehavior, ISlotContainer<TSlot, TItem>
        where TSlot : class, ISlot
        where TItem : class, ISlotItem<TSlot>
        where TImpl : class, TItem
        where TList : ReorderableArray<TImpl>
    {
        public abstract IDirectory<TSlot> Slots { get; }

        private IDictionary<string, TImpl> _mappings;

        [SerializeField, Reorderable] private TList _items;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // ReSharper disable once InconsistentlySynchronizedField
            _mappings = _items
                .Where(c => Slots.Contains(c.Slot.Name))
                .ToDictionary(i => i.Name);
        }

        public bool Contains(string key) => _mappings.ContainsKey(key);

        public TItem this[string key]
        {
            get
            {
                TImpl value;

                return _mappings.TryGetValue(key, out value) ? value : null;
            }
        }

        public void Set(TItem item)
        {
            Assert.IsNotNull(item, "item != null");

            var slot = ValidateSlot(item.Slot.Name);
            var entry = ValidateItem(item);

            lock (_mappings)
            {
                AddItem(entry);

                _mappings.Add(slot, entry);
                _items.Add(entry);
            }
        }

        public void Remove(string key)
        {
            Assert.IsNotNull(key, "key != null");

            var slot = ValidateSlot(key);
            var item = this[slot];

            if (item == null) return;

            lock (_mappings)
            {
                var impl = ValidateItem(item);

                RemoveItem(impl);

                _mappings.Remove(slot);

                var items = _items.Where(c => c.Slot.Name == slot).ToList();

                items.Reverse();
                items.ForEach(i => _items.Remove(i));
            }
        }

        protected abstract void AddItem(TImpl item);

        protected abstract void RemoveItem(TImpl item);

        protected virtual string ValidateSlot(string key)
        {
            if (!Slots.Contains(key))
            {
                throw new ArgumentException($"Invalid slot name: '{key}'.");
            }

            return key;
        }

        protected virtual TImpl ValidateItem(TItem item)
        {
            var impl = item as TImpl;

            if (impl == null)
            {
                throw new ArgumentException($"Invalid item type: '{item}'.");
            }

            return impl;
        }
    }
}