using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Alensia.Core.UI
{
    [ExecuteInEditMode]
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

        protected virtual IList<Object> Peers => new List<Object>();

        protected virtual HideFlags PeerFlags => HideFlags.HideInHierarchy | HideFlags.HideInInspector;

        public virtual void Initialize(IUIContext context)
        {
            Assert.IsNotNull(context, "context != null");

            lock (this)
            {
                if (Context != null)
                {
                    throw new InvalidOperationException(
                        $"The component has already been initialized: '{Name}'.");
                }
            }

            Context = context;
        }

        protected virtual void InitializePeers()
        {
        }

        protected virtual void ApplyHideFlags()
        {
            foreach (var peer in Peers)
            {
                peer.hideFlags = PeerFlags;
            }
        }

        protected virtual void ValidateProperties()
        {
        }

//TODO It seems that those 'magic methods' of MonoBehaviour confuse the hell out of the compiler, so it we remove this method, the player build fails.  
#pragma warning disable 108,114
        protected virtual void Awake()
        {
            InitializePeers();
            ApplyHideFlags();
        }

        protected virtual void Reset()
        {
            InitializePeers();
            ApplyHideFlags();
        }

        protected virtual void OnValidate()
        {
            if (Context != null) return;

            ApplyHideFlags();
            ValidateProperties();
        }
#pragma warning restore 108,114

        public void Show() => Visible = true;

        public void Hide() => Visible = false;
    }
}