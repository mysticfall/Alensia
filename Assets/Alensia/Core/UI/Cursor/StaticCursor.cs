using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Cursor
{
    [Serializable]
    public class StaticCursor : CursorDefinition
    {
        public Texture2D Image => _image;

        [SerializeField]
        private Texture2D _image;

        protected StaticCursor()
        {
        }

        public StaticCursor(string name, Vector2 hotspot, Texture2D image) : base(name, hotspot)
        {
            Assert.IsNotNull(image, "image != null");

            _image = image;
        }

        public override Vector2 Size => Image == null ? Vector2.zero : new Vector2(Image.width, Image.height);

        public override UniRx.IObservable<Texture2D> Create() => Observable.Return(Image);
    }
}