using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Collection;
using Alensia.Core.Common;
using Malee;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Item
{
    public abstract class ClothingContainer<TItem, TList> : ManagedMonoBehavior, IClothingContainer
        where TItem : class, IClothing
        where TList : ReorderableArray<TItem>
    {
        protected abstract IRace Race { get; }

        public IDirectory<ClothingSlot> Slots => Race.ClothingSlots;

        private IDictionary<string, TItem> _mappings;

        [SerializeField, Reorderable] private TList _clothings;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // ReSharper disable once InconsistentlySynchronizedField
            _mappings = _clothings
                .Where(c => Slots.Contains(c.Slot.Name))
                .ToDictionary(i => i.Name);
        }

        public bool Contains(string key) => _mappings.ContainsKey(key);

        public IClothing this[string key]
        {
            get
            {
                TItem value;

                return _mappings.TryGetValue(key, out value) ? value : null;
            }
        }

        public void Set(IClothing item)
        {
            Assert.IsNotNull(item, "item != null");

            var slot = CheckSlot(item.Slot.Name);
            var cloth = CheckItem(item);

            lock (_mappings)
            {
                ApplyClothing(cloth);

                _mappings.Add(slot, cloth);
                _clothings.Add(cloth);
            }
        }

        public void Remove(string key)
        {
            Assert.IsNotNull(key, "key != null");

            var slot = CheckSlot(key);
            var item = this[slot];

            if (item == null) return;

            lock (_mappings)
            {
                var cloth = CheckItem(item);

                RemoveClothing(slot, cloth);

                _mappings.Remove(slot);

                var items = _clothings.Where(c => c.Slot.Name == slot).ToList();

                items.Reverse();
                items.ForEach(i => _clothings.Remove(i));
            }
        }

        protected abstract void ApplyClothing(TItem item);

        protected abstract void RemoveClothing(string key, TItem item);

        private string CheckSlot(string key)
        {
            if (!Slots.Contains(key))
            {
                throw new ArgumentException($"Invalid slot name: '{key}'.");
            }

            return key;
        }

        private TItem CheckItem(IClothing item)
        {
            var cloth = item as TItem;

            if (cloth == null)
            {
                throw new ArgumentException(
                    $"'{nameof(item)}' is not a valid clothing type.'");
            }

            return cloth;
        }
    }
}