using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Screen
{
    [RequireComponent(typeof(Canvas))]
    public class Screen : UIElement, IScreen
    {
        public IEnumerable<IComponentHandler> Items => Transform.GetChildren<IComponentHandler>().ToList();

        public IReadOnlyDictionary<string, ScreenItemDefinition> ItemDefinitions { get; private set; }

        public Canvas Canvas { get; private set; }

        [SerializeField] private ScreenItemDefinition[] _uiItems;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Canvas = GetComponent<Canvas>();

            if (!Application.isPlaying) return;

            Assert.IsNotNull(
                Canvas,
                $"No Canvas component attached to the screen: '{Name}'.");

            ItemDefinitions = _uiItems?.ToDictionary(i => i.Name, i => i) ??
                              new Dictionary<string, ScreenItemDefinition>();

            CreateInitialItems();

            foreach (var component in Items.Select(i => i.Component))
            {
                component.Initialize(context);
            }
        }

        public T FindUI<T>() where T : class, IComponentHandler => Items.OfType<T>().FirstOrDefault();

        public T FindUI<T>(string key) where T : class, IComponentHandler =>
            Items.FirstOrDefault(i => i.Name == key) as T;

        public T ShowUI<T>(string key) where T : class, IComponentHandler
        {
            var definition = ItemDefinitions[key];

            T handler = null;

            lock (this)
            {
                if (definition.Singleton)
                {
                    handler = FindUI<T>(key);
                }

                if (handler != null) return handler;

                handler = Instantiate<T>(definition);
            }

            return handler;
        }

        protected virtual void CreateInitialItems()
        {
            var items = Items.ToDictionary(i => i.Name, i => i);

            ItemDefinitions.Values
                .Where(i => i.Enable && !items.ContainsKey(i.Name))
                .ToList()
                .ForEach(d => Instantiate<IComponentHandler>(d));
        }

        protected virtual T Instantiate<T>(ScreenItemDefinition definition)
            where T : IComponentHandler
        {
            T ui;

            var item = definition.Item;

            if (item.scene != GameObject.scene)
            {
                ui = Instantiate(item, Transform).GetComponent<T>();
            }
            else
            {
                ui = item.GetComponent<T>();

                if (item.transform.parent != Transform)
                {
                    item.transform.SetParent(Transform);
                }
            }

            if (ui == null)
            {
                throw new ArgumentException(
                    $"Unable to find a suitable component on object: '{item.name}'.");
            }

            ui.GameObject.name = definition.Name;
            ui.Visible = definition.Enable;

            if (ui.Component.Context == null)
            {
                ui.Component.Initialize(Context);
            }

            return ui;
        }
    }
}