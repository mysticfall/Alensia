using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.I18n;
using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Screen;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UECursor = UnityEngine.Cursor;
using Zenject;

namespace Alensia.Core.UI
{
    public class UIManager : BaseObject, IUIManager
    {
        public IUIContext Context { get; }

        public IReadOnlyList<IScreen> Screens => ScreenRoot.GetComponents<IScreen>();

        public IReadOnlyDictionary<string, ScreenDefinition> ScreenDefinitions { get; }

        public Transform ScreenRoot { get; }

        public CursorSet CursorSet => Style?.CursorSet;

        public CursorState CursorState
        {
            get { return CursorState.Current; }
            set { value?.Apply(); }
        }

        public string DefaultCursor { get; set; }

        public UIStyle Style
        {
            get { return _style.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _style.Value = value;
            }
        }

        private readonly IReactiveProperty<UIStyle> _style;

        private IDisposable _cursor;

        public UIManager(
            Settings settings,
            ITranslator translator,
            DiContainer container)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(translator, "translator != null");
            Assert.IsNotNull(container, "container != null");

            _style = new UIStyleReactiveProperty(settings.Style);

            ScreenRoot = settings.ScreenRoot;
            ScreenDefinitions = settings.Screens?.ToDictionary(i => i.Name, i => i) ??
                                new Dictionary<string, ScreenDefinition>();

            Context = new UIContext(_style, translator, container);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            CreateInitialScreens();

            Context
                .ObserveEveryValueChanged(ctx => ctx.ActiveComponent)
                .Where(_ => Initialized)
                .Select(c => c?.Cursor ?? CursorNames.Default)
                .Where(c => CursorSet != null && CursorSet.Contains(c))
                .Select(c => CursorSet[c])
                .Subscribe(UpdateCursor)
                .AddTo(this);
        }

        protected override void OnDisposed()
        {
            _cursor?.Dispose();
            _cursor = null;

            base.OnDisposed();
        }


        protected virtual void CreateInitialScreens()
        {
            var screens = Screens.ToDictionary(s => s.Name, s => s);

            ScreenDefinitions.Values
                .Where(s => !screens.ContainsKey(s.Name))
                .ToList()
                .ForEach(s => CreateScreen(s, ScreenRoot));
        }

        protected virtual IScreen CreateScreen(ScreenDefinition definition, Transform parent)
        {
            return Context.Instantiate<ScreenDefinition, IScreen>(definition, parent);
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
            var screen = FindScreen(name);

            if (screen != null && screen.Visible)
            {
                screen.Visible = false;
            }
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
                    UnityEngine.Cursor.SetCursor(image, pos, CursorMode.ForceSoftware);
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