using System;
using Alensia.Core.Collection;
using Malee;
using UnityEngine;

namespace Alensia.Core.Character.Morph
{
    [Serializable]
    public class MorphableRace : Race, IMorphableRace
    {
        public IDirectory<BodyPartSlot> BodyPartSlots 
        {
            get
            {
                lock (this)
                {
                    if (_bodyPartMap != null) return _bodyPartMap;

                    _bodyPartMap = new SimpleDirectory<BodyPartSlot>(_bodyPartSlots);

                    return _bodyPartMap;
                }
            }
        }

        [SerializeField, Reorderable] private BodyPartSlotList _bodyPartSlots;

        private IDirectory<BodyPartSlot> _bodyPartMap;

        private void OnValidate() => _bodyPartMap = null;
    }
}