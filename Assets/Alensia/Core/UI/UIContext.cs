using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.I18n;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Screen;
using Malee;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Object = UnityEngine.Object;
using UECursor = UnityEngine.Cursor;

namespace Alensia.Core.UI
{
    public class UIContext : ManagedMonoBehavior, IRuntimeUIContext
    {
        [Inject]
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

        public CultureInfo Locale => Translator?.LocaleService?.Locale;

        [Inject]
        public ITranslator Translator { get; }

        public Transform ScreenRoot => _screenRoot ?? transform;

        public IEnumerable<IScreen> Screens => ScreenRoot.GetComponents<IScreen>();

        public IReadOnlyDictionary<string, ScreenDefinition> ScreenDefinitions { get; private set; }

        public IInteractableComponent ActiveComponent
        {
            get { return _activeComponent.Value; }
            set { _activeComponent.Value = value; }
        }

        public CursorState CursorState
        {
            get { return _cursorState; }
            set
            {
                value?.Apply();
                _cursorState = value;
            }
        }

        public string DefaultCursor
        {
            get { return _defaultCursor; }
            set { _defaultCursor = value; }
        }

        public IObservable<CultureInfo> OnLocaleChange => Translator?.LocaleService?.OnLocaleChange;

        public IObservable<IInteractableComponent> OnActiveComponentChange => _activeComponent?.Where(_ => Initialized);

        public IObservable<UIStyle> OnStyleChange => _style;

        [SerializeField] private UIStyleReactiveProperty _style;

        [SerializeField] private Transform _screenRoot;

        [SerializeField, Reorderable] private ScreenDefinitionList _screens;

        [SerializeField, PredefinedLiteral(typeof(CursorNames))] private string _defaultCursor;

        [SerializeField] private CursorState _cursorState;

        private readonly IReactiveProperty<IInteractableComponent> _activeComponent;

        private IDisposable _cursor;

        public UIContext()
        {
            _style = new UIStyleReactiveProperty();
            _activeComponent = new ReactiveProperty<IInteractableComponent>();

            _defaultCursor = CursorNames.Default;
            _cursorState = CursorState.Vislbe;
        }

        protected override void OnInitialized()
        {
            ScreenDefinitions = _screens?.ToDictionary(i => i.Name, i => i) ??
                                new Dictionary<string, ScreenDefinition>();

            CreateScreens();

            var whenNull = OnActiveComponentChange
                .Where(c => c == null)
                .Select(c => (string) null);

            var whenNotNull = OnActiveComponentChange
                .Where(c => c != null)
                .SelectMany(c => c.OnCursorChange);

            whenNotNull.Merge(whenNull)
                .Select(c => c ?? CursorNames.Default)
                .Select(FindCursor)
                .Where(c => c != null)
                .Subscribe(UpdateCursor, Debug.LogError)
                .AddTo(this);

            CursorState?.Apply();

            base.OnInitialized();
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

        public virtual IScreen FindScreen(string screen)
        {
            return ScreenRoot.Find(screen)?.GetComponent<IScreen>();
        }

        public virtual void ShowScreen(string screen)
        {
            var instance = FindScreen(screen);

            if (instance != null && !instance.Visible)
            {
                instance.Visible = true;
            }
        }

        public virtual void HideScreen(string screen)
        {
            Assert.IsNotNull(screen, "name != null");

            var instance = FindScreen(screen);

            if (instance != null && instance.Visible)
            {
                instance.Visible = false;
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

        protected virtual CursorDefinition FindCursor(string cursor)
        {
            var cursors = Style?.CursorSet;
            var key = cursor ?? DefaultCursor;

            if (cursors == null || key == null) return null;

            return cursors[key] ?? (DefaultCursor != null ? cursors[DefaultCursor] : null);
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
    }
}