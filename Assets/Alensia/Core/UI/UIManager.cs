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

        public IReadOnlyList<IComponent> Components => _children;

        private readonly List<IComponent> _children = new List<IComponent>();

        private readonly Settings _settings;

        public UIManager(Settings settings)
        {
            Assert.IsNotNull(settings, "settings != null");

            _settings = settings;
        }

        public bool Contains(IComponent child)
        {
            lock (this)
            {
                return _children.Contains(child);
            }
        }

        public void Add(IComponent child)
        {
            Assert.IsNotNull(child, "child != null");

            lock (this)
            {
                if (_children.Contains(child)) return;

                _children.Add(child);
            }
        }

        public void Remove(IComponent child)
        {
            Assert.IsNotNull(child, "child != null");

            lock (this)
            {
                _children.Remove(child);
            }
        }

        public virtual void RemoveAll()
        {
            lock (this)
            {
                _children.Clear();
            }
        }

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