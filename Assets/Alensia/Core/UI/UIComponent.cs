using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.I18n;
using Alensia.Core.UI.Property;
using Alensia.Core.UI.Screen;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public abstract class UIComponent : UIElement, IComponent
    {
        public IScreen Screen => transform.GetComponentInParent<IScreen>();

        public IComponent Parent => transform.parent?.GetComponent<IComponent>();

        public IEnumerable<IComponent> Ancestors => new AncestorEnumerable(this);

        public IEnumerable<IComponentHandler> Handlers => GetComponents<IComponentHandler>();

        public UIStyle Style
        {
            get { return _style.Value ?? Context?.Style; }
            set { _style.Value = value; }
        }

        public IObservable<UIStyle> OnStyleChange => _style;

        protected virtual TextStyle DefaultTextStyle => Style?.TextStyles?["Text"];

        protected virtual ImageAndColor DefaultBackground => Style?.ImagesAndColors?["Background"];

        [SerializeField] private UIStyleReactiveProperty _style;

        private readonly CompositeDisposable _observers = new CompositeDisposable();

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            context.OnStyleChange
                .Subscribe(OnStyleChanged, Debug.LogError)
                .AddTo(_observers);

            context.OnLocaleChange
                .Subscribe(OnLocaleChanged, Debug.LogError)
                .AddTo(_observers);

            if (Application.isPlaying)
            {
                InitializeComponent(context, true);
                InitializeHandlers(context);
            }
            else
            {
                InitializeComponent(context, false);
            }
        }

        protected virtual void InitializeComponent(IUIContext context, bool isPlaying)
        {
        }

        protected virtual void InitializeHandlers(IUIContext context)
        {
            Handlers
                .Where(h => h.Context == null)
                .ToList()
                .ForEach(h => h.Initialize(Context));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Context != null || Application.isPlaying) return;

            var context = CreateEditorUIContext();

            if (context != null)
            {
                Initialize(context);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (!Application.isPlaying)
            {
                _observers.Clear();
            }
        }

        protected virtual IUIContext CreateEditorUIContext()
        {
            return Resources.Load<EditorUIContext>("UI/EditorUIContext");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _observers.Clear();
        }

        protected override void Reset()
        {
            base.Reset();

            var source = CreatePristineInstance();

            ResetFromInstance(source);

            DestroyImmediate(source.gameObject);

            OnEditorUpdate();
        }

        protected virtual void ResetFromInstance(UIComponent component)
        {
        }

        protected abstract UIComponent CreatePristineInstance();

        protected override void OnEditorUpdate()
        {
            base.OnEditorUpdate();

            OnLocaleChanged(Context?.Locale);
            OnStyleChanged(Style);
        }

        protected virtual void OnStyleChanged(UIStyle style)
        {
        }

        protected virtual void OnLocaleChanged(CultureInfo locale)
        {
        }

        protected virtual void UpdatePeer(Text peer, TranslatableText text)
        {
            var translator = Context?.Translator;
            var value = translator == null ? text.Text : text.Translate(translator);

            peer.text = value;
        }
    }

    internal class AncestorEnumerable : IEnumerable<IComponent>
    {
        private readonly IComponent _parent;

        public AncestorEnumerable(IComponent parent)
        {
            _parent = parent;
        }

        public IEnumerator<IComponent> GetEnumerator() => new AncestorEnumerator(_parent);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class AncestorEnumerator : IEnumerator<IComponent>
        {
            public IComponent Current { get; private set; }

            object IEnumerator.Current => Current;

            private readonly IComponent _parent;

            public AncestorEnumerator(IComponent parent)
            {
                _parent = parent;

                Reset();
            }

            public bool MoveNext()
            {
                lock (this)
                {
                    Current = Current?.Parent;

                    return Current != null;
                }
            }

            public void Reset() => Current = _parent;

            public void Dispose()
            {
            }
        }
    }
}