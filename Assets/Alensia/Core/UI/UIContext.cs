using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.I18n;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Screen;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Object = UnityEngine.Object;
using UECursor = UnityEngine.Cursor;

namespace Alensia.Core.UI
{
    public class UIContext : BaseObject, IUIContext
    {
        public DiContainer DiContainer { get; }

        public UIStyle Style
        {
            get { return _style.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _style.Value = value;
            }
        }

        public CultureInfo Locale => Translator?.LocaleService?.CurrentLocale;

        public ITranslator Translator { get; }

        public Transform ScreenRoot { get; }

        public IReadOnlyList<IScreen> Screens => ScreenRoot.GetComponents<IScreen>();

        public IReadOnlyDictionary<string, ScreenDefinition> ScreenDefinitions { get; }

        public IInteractableComponent ActiveComponent
        {
            get { return _activeComponent.Value; }
            set { _activeComponent.Value = value; }
        }

        public CursorState CursorState
        {
            get { return CursorState.Current; }
            set { value?.Apply(); }
        }

        public string DefaultCursor { get; set; }

        public IObservable<CultureInfo> OnLocaleChange => Translator.LocaleService.OnLocaleChange;

        public IObservable<IInteractableComponent> OnActiveComponentChange => _activeComponent.Where(_ => Initialized);

        public IObservable<UIStyle> OnStyleChange => _style;

        private readonly IReactiveProperty<UIStyle> _style;

        private readonly IReactiveProperty<IInteractableComponent> _activeComponent;

        private IDisposable _cursor;

        public UIContext(
            Settings settings,
            ITranslator translator,
            DiContainer container)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(translator, "translator != null");
            Assert.IsNotNull(container, "container != null");

            Translator = translator;
            DiContainer = container;

            _style = new UIStyleReactiveProperty(settings.Style);
            _activeComponent = new ReactiveProperty<IInteractableComponent>();

            ScreenRoot = settings.ScreenRoot;
            ScreenDefinitions = settings.Screens?.ToDictionary(i => i.Name, i => i) ??
                                new Dictionary<string, ScreenDefinition>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            CreateScreens();

            var whenNull = OnActiveComponentChange
                .Where(c => c == null)
                .Select(c => (string) null);

            var whenNotNull = OnActiveComponentChange
                .Where(c => c != null)
                .SelectMany(c => c.OnCursorChange);

            whenNotNull.Merge(whenNull)
                .Select(c => c ?? CursorNames.Default)
                .Merge(OnActiveComponentChange.Where(c => c == null).Select(_ => CursorNames.Default))
                .Select(c => Style?.CursorSet?[c])
                .Subscribe(UpdateCursor)
                .AddTo(this);
        }

        protected virtual void CreateScreens()
        {
            var active = Screens.ToDictionary(s => s.Name, s => s);

            ScreenDefinitions.Values
                .Where(s => !active.ContainsKey(s.Name))
                .ToList()
                .ForEach(CreateScreen);
        }

        protected virtual void CreateScreen(ScreenDefinition definition)
        {
            var item = definition.Item;

            IScreen screen;

            if (item.scene != ScreenRoot.gameObject.scene)
            {
                screen = Object.Instantiate(item, ScreenRoot).GetComponent<IScreen>();
            }
            else
            {
                screen = item.GetComponent<IScreen>();

                if (item.transform.parent != ScreenRoot)
                {
                    item.transform.SetParent(ScreenRoot);
                }
            }

            if (screen == null)
            {
                Debug.LogWarning($"Missing IScreen component on object: '{item.name}'.");
            }

            screen?.Initialize(this);
        }

        public virtual IScreen FindScreen(string name) => ScreenRoot.Find(name)?.GetComponent<IScreen>();

        public virtual void ShowScreen(string name)
        {
            var screen = FindScreen(name);

            if (screen != null && !screen.Visible)
            {
                screen.Visible = true;
            }
        }

        public virtual void HideScreen(string name)
        {
            Assert.IsNotNull(name, "name != null");

            var screen = FindScreen(name);

            if (screen != null && screen.Visible)
            {
                screen.Visible = false;
            }
        }

        public T Instantiate<T>(GameObject prefab, Transform parent) where T : IUIElement
        {
            T ui;

            if (prefab.scene != parent.gameObject.scene)
            {
                ui = Object.Instantiate(prefab, parent).GetComponent<T>();
            }
            else
            {
                ui = prefab.GetComponent<T>();

                if (prefab.transform.parent != parent)
                {
                    prefab.transform.SetParent(parent);
                }
            }

            if (ui == null)
            {
                throw new ArgumentException(
                    $"Unable to find a suitable component on object: '{prefab.name}'.");
            }

            var handler = ui as IComponentHandler;

            if (handler == null)
            {
                ui.Initialize(this);
            }
            else
            {
                handler.Component.Initialize(this);
            }

            return ui;
        }

        protected override void OnDisposed()
        {
            _cursor?.Dispose();
            _cursor = null;

            base.OnDisposed();
        }

        protected virtual void UpdateCursor(CursorDefinition cursor)
        {
            lock (this)
            {
                _cursor?.Dispose();
                _cursor = cursor.Create().SubscribeWithState(cursor.Hotspot, (image, pos) =>
                {
                    //TODO Can't use CursorMode.Auto, because it doesn't work on Linux yet.
                    //(See: https://forum.unity3d.com/threads/cursor-setcursor-does-not-work-in-editor.476617/)
                    UECursor.SetCursor(image, pos, CursorMode.ForceSoftware);
                });
            }
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
            public UIStyle Style;

            public Transform ScreenRoot;

            public ScreenDefinition[] Screens;
        }
    }
}