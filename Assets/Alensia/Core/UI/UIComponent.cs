using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.UI.Cursor;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using static System.String;

namespace Alensia.Core.UI
{
    public abstract class UIComponent : UIElement, IComponent
    {
        public IComponent Parent => transform.parent?.GetComponent<IComponent>();

        public IEnumerable<IComponent> Ancestors => new AncestorEnumerable(this);

        public string Cursor
        {
            get { return IsNullOrWhiteSpace(_cursor.Value) ? Parent?.Cursor : _cursor.Value; }
            set { _cursor.Value = value?.Trim(); }
        }

        public IObservable<PointerEventData> OnPointerEnter => this.OnPointerEnterAsObservable();

        public IObservable<PointerEventData> OnPointerExit => this.OnPointerExitAsObservable();

        [SerializeField, PredefinedLiteral(typeof(CursorNames))] private StringReactiveProperty _cursor;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            OnPointerEnter
                .Where(_ => !HasActiveChild())
                .Subscribe(_ => Context.ActiveComponent = this)
                .AddTo(this);

            OnPointerExit
                .AsUnitObservable()
                .Merge(OnHide)
                .Where(_ => ReferenceEquals(Context.ActiveComponent, this))
                .Subscribe(_ => Context.ActiveComponent = FirstActiveAncestor)
                .AddTo(this);
        }

        protected override void Reset()
        {
            base.Reset();

            var source = CreatePristineInstance();

            ResetFromInstance(source);

            DestroyImmediate(source.gameObject);
        }

        protected virtual void ResetFromInstance(UIComponent component)
        {
        }

        protected abstract UIComponent CreatePristineInstance();

        private bool HasActiveChild()
        {
            var active = Context.ActiveComponent;

            return active?.Ancestors.FirstOrDefault(c => ReferenceEquals(c, this)) != null;
        }

        private IComponent FirstActiveAncestor => Ancestors.FirstOrDefault(a => a.Visible);
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