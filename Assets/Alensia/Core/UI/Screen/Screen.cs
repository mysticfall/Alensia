using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI.Screen
{
    [RequireComponent(typeof(Canvas))]
    public class Screen : UIElement, IScreen
    {
        public IReadOnlyList<IUIHandler> Items => GetComponentsInChildren<IUIHandler>();

        public IReadOnlyDictionary<string, ScreenItemDefinition> ItemDefinitions { get; private set; }

        public Canvas Canvas { get; private set; }

        [SerializeField] private ScreenItemDefinition[] _uiItems;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            Canvas = GetComponent<Canvas>();

            Assert.IsNotNull(
                Canvas,
                $"No Canvas component attached to the screen: '{Name}'.");

            ItemDefinitions = _uiItems?.ToDictionary(i => i.Name, i => i) ??
                              new Dictionary<string, ScreenItemDefinition>();

            CreateInitialItems();
        }

        protected virtual void CreateInitialItems()
        {
            var items = Items.ToDictionary(i => i.Name, i => i);

            ItemDefinitions.Values
                .Where(i => i.Enable && !items.ContainsKey(i.Name))
                .ToList()
                .ForEach(d => CreateItem<IUIHandler>(d));
        }

        protected virtual T CreateItem<T>(ScreenItemDefinition definition) where T : IUIHandler
        {
            return Context.Instantiate<ScreenItemDefinition, T>(definition, transform);
        }

        public T FindUI<T>() where T : class, IUIHandler => GetComponentInChildren<T>();

        public T FindUI<T>(string key) where T : class, IUIHandler => transform.Find(key)?.GetComponent<T>();

        public T ShowUI<T>(string key) where T : class, IUIHandler
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

                handler = CreateItem<T>(definition);
            }

            return handler;
        }
    }
}