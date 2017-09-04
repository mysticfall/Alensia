using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Property;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public class UIStyle : ScriptableObject, INamed, IEditorSettings
    {
        public string Name => name;

        public UIStyle Parent => _parent;

        public CursorSet CursorSet => _cursorSet;

        public IDirectory<ImageAndColor> ImagesAndColors => _imagesAndColorsLookup;

        public IDirectory<ImageAndColorSet> ImageAndColorSets => _imageAndColorSetLookup;

        public IDirectory<TextStyle> TextStyles => _textStyleLookup;

        public IDirectory<TextStyleSet> TextStyleSets => _textStyleSetLookup;

        [SerializeField] private UIStyle _parent;

        [SerializeField] private CursorSet _cursorSet;

        [SerializeField] private ImageAndColorItem[] _imagesAndColors;

        [SerializeField] private ImageAndColorSetItem[] _imageAndColorSets;

        [SerializeField] private TextStyleItem[] _textStyles;

        [SerializeField] private TextStyleSetItem[] _textStyleSets;

        [NonSerialized] private StyleItemLookup<ImageAndColorItem, ImageAndColor> _imagesAndColorsLookup;

        [NonSerialized] private StyleItemLookup<ImageAndColorSetItem, ImageAndColorSet> _imageAndColorSetLookup;

        [NonSerialized] private StyleItemLookup<TextStyleItem, TextStyle> _textStyleLookup;

        [NonSerialized] private StyleItemLookup<TextStyleSetItem, TextStyleSet> _textStyleSetLookup;

        internal EditorUIContext EditorUIContext;

        private void OnValidate()
        {
            UpdateItems();

            EditorUIContext?.RefreshStyle();
        }

        private void OnEnable() => UpdateItems();

        private void UpdateItems()
        {
            _imagesAndColors = _imagesAndColors?.OrderBy(i => i.Name).ToArray();
            _imageAndColorSets = _imageAndColorSets?.OrderBy(i => i.Name).ToArray();

            _textStyles = _textStyles?.OrderBy(i => i.Name).ToArray();
            _textStyleSets = _textStyleSets?.OrderBy(i => i.Name).ToArray();

            _imagesAndColorsLookup = new StyleItemLookup<ImageAndColorItem, ImageAndColor>(
                _imagesAndColors, _parent?._imagesAndColorsLookup);
            _imageAndColorSetLookup = new StyleItemLookup<ImageAndColorSetItem, ImageAndColorSet>(
                _imageAndColorSets, _parent?._imageAndColorSetLookup);

            _textStyleLookup = new StyleItemLookup<TextStyleItem, TextStyle>(
                _textStyles, _parent?._textStyleLookup);
            _textStyleSetLookup = new StyleItemLookup<TextStyleSetItem, TextStyleSet>(
                _textStyleSets, _parent?._textStyleSetLookup);
        }
    }

    internal class StyleItemLookup<TItem, TValue> : IDirectory<TValue>
        where TItem : UIStyleItem<TValue>
        where TValue : class
    {
        public IDirectory<TValue> Parent { get; }

        private readonly IDictionary<string, TValue> _dictionary;

        public StyleItemLookup(IEnumerable<TItem> items, IDirectory<TValue> parent)
        {
            Parent = parent;

            var list = items?
                .GroupBy(i => i.Name)
                .ToDictionary(g => g.Key, g => g.First().Value);

            _dictionary = list ?? new Dictionary<string, TValue>();
        }

        public bool Contains(string key)
        {
            Assert.IsNotNull(key, "key != null");

            var found = _dictionary.ContainsKey(key);

            return found || Parent != null && Parent.Contains(key);
        }

        public TValue this[string key]
        {
            get
            {
                Assert.IsNotNull(key, "key != null");

                TValue value;

                _dictionary.TryGetValue(key, out value);

                return value ?? Parent?[key];
            }
        }
    }

    internal abstract class UIStyleItem<T> : INamed where T : class
    {
        public string Name => _name;

        public T Value => _value;

        [SerializeField] private string _name;

        [SerializeField] private T _value;

        protected UIStyleItem(string name, T value)
        {
            Assert.IsNotNull(name, "name != null");
            Assert.IsNotNull(value, "value != null");

            _name = name;
            _value = value;
        }
    }

    [Serializable]
    internal class TextStyleItem : UIStyleItem<TextStyle>
    {
        internal TextStyleItem(string name, TextStyle value) : base(name, value)
        {
        }
    }

    [Serializable]
    internal class TextStyleSetItem : UIStyleItem<TextStyleSet>
    {
        internal TextStyleSetItem(string name, TextStyleSet value) : base(name, value)
        {
        }
    }

    [Serializable]
    internal class ImageAndColorItem : UIStyleItem<ImageAndColor>
    {
        internal ImageAndColorItem(string name, ImageAndColor value) : base(name, value)
        {
        }
    }

    [Serializable]
    internal class ImageAndColorSetItem : UIStyleItem<ImageAndColorSet>
    {
        internal ImageAndColorSetItem(string name, ImageAndColorSet value) : base(name, value)
        {
        }
    }

    [Serializable]
    public class UIStyleReactiveProperty : ReactiveProperty<UIStyle>
    {
        public UIStyleReactiveProperty()
        {
        }

        public UIStyleReactiveProperty(
            UIStyle initialValue) : base(initialValue)
        {
        }

        public UIStyleReactiveProperty(UniRx.IObservable<UIStyle> source) : base(source)
        {
        }

        public UIStyleReactiveProperty(UniRx.IObservable<UIStyle> source,
            UIStyle initialValue) : base(source, initialValue)
        {
        }
    }
}