using Alensia.Core.Common;
using Alensia.Core.I18n;
using Alensia.Core.Item.Generic;
using UnityEngine;

namespace Alensia.Core.Item
{
    public abstract class Clothing<T> : ManagedMonoBehavior, IClothing<T> where T : IClothingForm
    {
        public string Name => Form.Name;

        public TranslatableText DisplayName => Form.DisplayName;

        public ClothingSlot Slot => Form.Slot;

        public T Form => _form;

        IClothingForm IFormInstance<IClothingForm>.Form => Form;

        [SerializeField] private T _form;
    }
}