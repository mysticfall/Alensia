using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Cursor
{
    public abstract class CursorDefinition : IEditorSettings
    {
        public string Name => _name;

        public abstract Vector2 Size { get; }

        public Vector2 Hotspot => _hotspot;

        public abstract IObservable<Texture2D> Create();

        [SerializeField] private string _name;

        [SerializeField] private Vector2 _hotspot;

        protected CursorDefinition()
        {
        }

        protected CursorDefinition(string name, Vector2 hotspot)
        {
            Assert.IsNotNull(name, "name != null");

            _name = name;
            _hotspot = hotspot;
        }
    }
}