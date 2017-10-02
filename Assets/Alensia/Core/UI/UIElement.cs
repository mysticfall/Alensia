using System;
using System.Collections.Generic;
using Alensia.Core.Common;
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

        public bool Valid => !IsDestroyed();

        public RectTransform RectTransform => _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());

        public Transform Transform => transform;

        public GameObject GameObject => gameObject;

        public IObservable<Unit> OnShow => this.OnEnableAsObservable();

        public IObservable<Unit> OnHide => this.OnDisableAsObservable();

        public IObservable<bool> OnVisibilityChange => OnShow.Select(_ => true).Merge(OnHide.Select(_ => false));

        public IObservable<Unit> OnRemove => this.OnDestroyAsObservable();

        protected virtual IList<Object> Peers => new List<Object>();

        protected virtual HideFlags PeerFlags => HideFlags.HideInHierarchy | HideFlags.HideInInspector;

        private RectTransform _rectTransform;

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

            Context.DiContainer?.Inject(this);
        }

        public virtual void Show() => Visible = true;

        public virtual void Hide() => Visible = false;

        public virtual void Remove() => Destroy(GameObject);

        protected override void OnDisable()
        {
            base.OnDisable();

            if (!Application.isPlaying)
            {
                Context = null;
            }
        }

        protected virtual void OnEditorUpdate()
        {
        }

        protected virtual void ApplyHideFlags()
        {
            foreach (var peer in Peers)
            {
                peer.hideFlags = PeerFlags;
            }
        }

        protected T FindPeer<T>(string path) where T : class => Transform.FindComponent<T>(path);

        protected T FindPeerInChildren<T>(string path) where T : class => Transform.FindComponentInChildren<T>(path);

//TODO It seems that those 'magic methods' of MonoBehaviour confuse the hell out of the compiler, so it we remove this method, the player build fails.  
#pragma warning disable 108,114
        protected virtual void Awake()
        {
            ApplyHideFlags();
        }

        protected virtual void Reset()
        {
            ApplyHideFlags();
        }

        protected virtual void OnValidate()
        {
            OnEditorUpdate();
            ApplyHideFlags();
        }
#pragma warning restore 108,114
    }
}