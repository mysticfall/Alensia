using System;
using Alensia.Core.Common;
using Alensia.Core.I18n;
using UnityEngine;
using UnityEngine.Assertions;
using UEDropdown = UnityEngine.UI.Dropdown;

namespace Alensia.Core.UI
{
    [Serializable]
    public class DropdownItem : IEditorSettings
    {
        public string Key => _key;

        public TranslatableText Label => _label;

        public Sprite Image => _image;

        [SerializeField] private string _key;

        [SerializeField] private TranslatableText _label;

        [SerializeField] private Sprite _image;

        public DropdownItem(
            string label,
            Sprite image = null) : this(label, new TranslatableText(label), image)
        {
        }

        public DropdownItem(
            string key,
            string label,
            Sprite image = null) : this(key, new TranslatableText(label), image)
        {
        }

        public DropdownItem(
            string key, TranslatableText label, Sprite image = null)
        {
            Assert.IsNotNull(key, "key != null");
            Assert.IsNotNull(label, "label != null");

            _key = key;
            _label = label;
            _image = image;
        }

        internal UEDropdown.OptionData AsOptionData(IUIContext context)
        {
            var translator = context?.Translator;
            var label = translator != null ? _label.Translate(translator) : _label.Text;

            return new UEDropdown.OptionData(label);
        }
    }
}