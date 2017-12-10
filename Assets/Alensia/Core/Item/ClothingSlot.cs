using System;
using Alensia.Core.Common;
using Malee;

namespace Alensia.Core.Item
{
    [Serializable]
    public class ClothingSlot : Form, ISlot
    {
    }

    [Serializable]
    internal class ClothingSlotList : ReorderableArray<ClothingSlot>
    {
    }
}