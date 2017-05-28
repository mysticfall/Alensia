using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.UI
{
    public class UIManager : IUIManager, IGuiRenderable
    {
        public GUISkin Skin
        {
            get { return _settings.Skin ?? GUI.skin; }
            set { _settings.Skin = value; }
        }

        public CursorDefinition ActiveCursor
        {
            get { return _activeCursor; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                if (_activeCursor == value) return;

                _activeCursor = value;

                value.Apply();
            }
        }

        public void ShowCursor() => ActiveCursor = _settings.DefaultCursor;

        public void HideCursor() => ActiveCursor = CursorDefinition.Hidden;

        public IReadOnlyList<IComponent> Components => _components.Components;

        public UniRx.IObservable<IComponent> ComponentAdded => _components.ComponentAdded;

        public UniRx.IObservable<IComponent> ComponentRemoved => _components.ComponentRemoved;

        private readonly IComponentsHolder _components = new ComponentsHolder();

        private readonly Settings _settings;

        private CursorDefinition _activeCursor;

        public UIManager(Settings settings)
        {
            Assert.IsNotNull(settings, "settings != null");

            _settings = settings;

            ActiveCursor = settings.DefaultCursor;
        }

        public bool Contains(IComponent child) => _components.Contains(child);

        public void Add(IComponent child) => _components.Add(child);

        public void Remove(IComponent child) => _components.Remove(child);

        public virtual void RemoveAll() => _components.RemoveAll();

        public void GuiRender()
        {
            lock (this)
            {
                var children = Components.ToList();

                children.ForEach(c => c.Paint());
            }
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
            public GUISkin Skin;

            public CursorDefinition DefaultCursor = CursorDefinition.Default;
        }
    }
}