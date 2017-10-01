using System;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    [Serializable]
    public class TranslatableText : IEditorSettings, ITranslatable
    {
        public string Text => _text;

        public string TextKey => _textKey;

        public bool Translatable => !string.IsNullOrEmpty(TextKey);

        [SerializeField] private string _text;

        [SerializeField] private string _textKey;

        public TranslatableText(string text) : this(text, null)
        {
        }

        public TranslatableText(string text, string textKey)
        {
            _text = text;
            _textKey = textKey;
        }

        public TranslatableText(TranslatableText source)
        {
            Assert.IsNotNull(source);

            _text = source.Text;
            _textKey = source.TextKey;
        }

        public string Translate(ITranslator translator) =>
            Translatable ? translator.Translate(TextKey) : Text;

        protected bool Equals(TranslatableText other)
        {
            return string.Equals(_text, other._text) && string.Equals(_textKey, other._textKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            return ReferenceEquals(this, obj) || Equals((TranslatableText) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Text != null ? Text.GetHashCode() : 0) * 397) ^
                       (TextKey != null ? TextKey.GetHashCode() : 0);
            }
        }
    }

    [Serializable]
    public class TranslatableTextReactiveProperty : ReactiveProperty<TranslatableText>
    {
        public TranslatableTextReactiveProperty()
        {
        }

        public TranslatableTextReactiveProperty(
            TranslatableText initialValue) : base(initialValue)
        {
        }

        public TranslatableTextReactiveProperty(IObservable<TranslatableText> source) : base(source)
        {
        }

        public TranslatableTextReactiveProperty(
            IObservable<TranslatableText> source, TranslatableText initialValue) : base(source, initialValue)
        {
        }
    }
}