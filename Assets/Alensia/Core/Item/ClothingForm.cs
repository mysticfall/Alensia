using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Item
{
    public abstract class ClothingForm : Form, IClothingForm
    {
        public ClothingSlot Slot => _slot;

        [SerializeField] private ClothingSlot _slot;
    }
}