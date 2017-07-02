using System;
using Alensia.Core.I18n;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.UI
{
    public class UIContext : IUIContext
    {
        public ITranslator Translator { get; }

        public DiContainer DiContainer { get; }

        public IComponent ActiveComponent { get; set; }

        public UIContext(ITranslator translator, DiContainer container)
        {
            Assert.IsNotNull(translator, "translator != null");
            Assert.IsNotNull(container, "container != null");

            Translator = translator;
            DiContainer = container;
        }

        public virtual TUI Instantiate<TDef, TUI>(TDef definition, Transform parent)
            where TDef : UIDefinition where TUI : IUIElement
        {
            TUI ui;

            var item = definition.Item;

            if (item.scene != parent.gameObject.scene)
            {
                ui = DiContainer.InstantiatePrefabForComponent<TUI>(item, parent);
            }
            else
            {
                ui = item.GetComponent<TUI>();

                if (item.transform.parent != parent)
                {
                    item.transform.SetParent(parent);
                }
            }

            if (ui == null)
            {
                throw new ArgumentException(
                    $"Unable to find a suitable component for UI: '{definition.Name}'.");
            }

            ui.GameObject.name = definition.Name;
            ui.Visible = definition.Enable;

            ui.Initialize(this);

            return ui;
        }
    }
}