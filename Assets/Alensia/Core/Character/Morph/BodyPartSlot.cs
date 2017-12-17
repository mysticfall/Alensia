using System;
using Alensia.Core.Common;
using Alensia.Core.Item;
using Malee;

namespace Alensia.Core.Character.Morph
{
    [Serializable]
    public class BodyPartSlot : Form, ISlot
    {
    }

    [Serializable]
    public class BodyPartSlotList : ReorderableArray<BodyPartSlot>
    {
    }
}