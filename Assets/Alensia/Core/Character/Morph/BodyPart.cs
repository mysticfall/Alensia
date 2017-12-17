using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Character.Morph
{
    public abstract class BodyPart : Form, IBodyPart
    {
        public BodyPartSlot Slot => _slot;

        [SerializeField] private BodyPartSlot _slot;
    }
}