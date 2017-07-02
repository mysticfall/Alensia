using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI
{
    public abstract class UIElement : UIBehaviour, IUIElement
    {
        public string Name => name;

        public IUIContext Context { get; private set; }

        public bool Visible
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Transform Transform => transform;

        public GameObject GameObject => gameObject;

        public UniRx.IObservable<Unit> OnShow => this.OnEnableAsObservable();

        public UniRx.IObservable<Unit> OnHide => this.OnDisableAsObservable();

        public UniRx.IObservable<bool> OnVisibilityChange =>
            OnShow.Select(_ => true).Merge(OnHide.Select(_ => false));

        private bool _initialized;

        public virtual void Initialize(IUIContext context)
        {
            Assert.IsNotNull(context, "context != null");

            lock (this)
            {
                if (_initialized)
                {
                    throw new InvalidOperationException(
                        $"The component has already been initialized: '{Name}'.");
                }

                _initialized = true;
            }

            Context = context;

            OnValidate();
        }

        //TODO It seems that those 'magic methods' of MonoBehaviour confuse the hell out of the compiler, so it we remove this method, the player build fails.  
#pragma warning disable 108,114
        protected virtual void OnValidate()
#pragma warning restore 108,114
        {
        }

        public void Show() => Visible = true;

        public void Hide() => Visible = false;
    }
}