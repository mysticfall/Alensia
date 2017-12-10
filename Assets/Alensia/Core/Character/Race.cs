using System;
using System.Collections.Generic;
using Alensia.Core.Collection;
using Alensia.Core.Common;
using Alensia.Core.Item;
using Malee;
using UnityEngine;

namespace Alensia.Core.Character
{
    [Serializable]
    public class Race : Form, IRace
    {
        public IEnumerable<Sex> Sexes => _sexes;

        public IDirectory<ClothingSlot> ClothingSlots 
        {
            get
            {
                lock (this)
                {
                    if (_clothingMap != null) return _clothingMap;

                    _clothingMap = new SimpleDirectory<ClothingSlot>(_clothingSlots);

                    return _clothingMap;
                }
            }
        }

        [SerializeField, Reorderable] private SexList _sexes;

        [SerializeField, Reorderable] private ClothingSlotList _clothingSlots;

        private IDirectory<ClothingSlot> _clothingMap;

        private void OnValidate() => _clothingMap = null;
    }

    [Serializable]
    internal class RaceList : ReorderableArray<Race>
    {
    }
}