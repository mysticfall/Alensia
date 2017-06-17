using System;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Cursor
{
    public abstract class CursorDefinition : ICursorDefinition, IEditorSettings
    {
        public string Name => _name;

        public abstract Vector2 Size { get; }

        public Vector2 Hotspot => _hotspot;

        public abstract IDisposable Apply();

        [SerializeField]
        private string _name;

        [SerializeField]
        private Vector2 _hotspot;

        protected CursorDefinition()
        {
        }

        protected CursorDefinition(string name, Vector2 hotspot)
        {
            Assert.IsNotNull(name, "name != null");

            _name = name;
            _hotspot = hotspot;
        }

        protected virtual void Apply(Texture2D image)
        {
            //TODO Can't use CursorMode.Auto, because it doesn't work on Linux yet.
            //(See: https://forum.unity3d.com/threads/cursor-setcursor-does-not-work-in-editor.476617/)
            UnityEngine.Cursor.SetCursor(image, Hotspot, CursorMode.ForceSoftware);
        }
    }
}