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

        public IReadOnlyList<IComponent> Components => _components.Components;

        public UniRx.IObservable<IComponent> ComponentAdded => _components.ComponentAdded;

        public UniRx.IObservable<IComponent> ComponentRemoved => _components.ComponentRemoved;

        private readonly IComponentsHolder _components = new ComponentHolder();

        private readonly Settings _settings;

        public UIManager(Settings settings)
        {
            Assert.IsNotNull(settings, "settings != null");

            _settings = settings;
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
        }
    }
}