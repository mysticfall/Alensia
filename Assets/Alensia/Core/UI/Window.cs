using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.UI.Property;
using Alensia.Core.UI.Resize;
using Alensia.Core.UI.Screen;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Alensia.Core.UI
{
    public class Window : Panel
    {
        public bool Movable
        {
            get { return _movable.Value; }
            set { _movable.Value = value; }
        }

        public bool Resizable
        {
            get { return _resizable.Value; }
            set { _resizable.Value = value; }
        }

        public bool Modal
        {
            get { return _modal.Value; }
            set { _modal.Value = value; }
        }

        public ImageAndColor Backdrop
        {
            get { return _backdrop.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _backdrop.Value = value;
            }
        }

        public DraggableHeader Header =>
            _header ?? (_header = Transform.Find("Header").GetComponent<DraggableHeader>());

        public Transform ContentPanel => _content ?? (_content = Transform.Find("Content"));

        public override IList<IComponent> Children => ContentPanel.GetChildren<IComponent>().ToList();

        protected virtual ImageAndColor DefaultBackdrop => Style?.ImagesAndColors?["Window.Backdrop"];

        protected VerticalLayoutGroup LayoutGroup =>
            _layoutGroup ?? (_layoutGroup = GetComponent<VerticalLayoutGroup>());

        protected Image BackdropImage => _backdropImage;

        protected override IList<Object> Peers
        {
            get
            {
                var peers = base.Peers;

                if (LayoutGroup != null) peers.Add(LayoutGroup);

                return peers;
            }
        }

        [SerializeField] private BoolReactiveProperty _movable;

        [SerializeField] private BoolReactiveProperty _resizable;

        [SerializeField] private BoolReactiveProperty _modal;

        [SerializeField] private ImageAndColorReactiveProperty _backdrop;

        [SerializeField, HideInInspector] private DraggableHeader _header;

        [SerializeField, HideInInspector] private VerticalLayoutGroup _layoutGroup;

        [NonSerialized] private Image _backdropImage;

        [NonSerialized] private Transform _content;

        private ResizeHelper _resizer;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            _movable
                .Where(_ => Header != null)
                .Subscribe(v => Header.Interactable = v)
                .AddTo(this);

            _resizer = new ResizeHelper(this);

            _resizer.Initialize();
            _resizer.Activate();

            _resizable
                .Where(_ => _resizer != null)
                .Subscribe(v => _resizer.Active = v)
                .AddTo(this);

            _backdrop
                .Where(_ => BackdropImage != null)
                .Subscribe(v => v.Update(BackdropImage, DefaultBackdrop))
                .AddTo(this);

            var parentStatusChanged = Transform
                .OnTransformParentChangedAsObservable()
                .Select(_ => Transform.parent != null);

            var hasValidParent = new ReactiveProperty<bool>(Transform.parent != null).Merge(parentStatusChanged);

            var visible = new ReactiveProperty<bool>(enabled).Merge(OnVisibilityChange);

            var shouldBackdropExists =
                Observable
                    .CombineLatest(_modal, visible, hasValidParent)
                    .Select(i => i.All(v => v))
                    .DistinctUntilChanged();

            shouldBackdropExists
                .Where(v => v && BackdropImage == null)
                .Subscribe(_ => ShowBackdrop())
                .AddTo(this);

            shouldBackdropExists
                .Where(v => !v && BackdropImage != null)
                .Subscribe(_ => HideBackdrop())
                .AddTo(this);
        }

        protected override void InitializeChildren(IUIContext context)
        {
            base.InitializeChildren(context);

            Header?.Initialize(context);

            Header?.OnDrag
                .Select(e => RectTransform.anchoredPosition + e.delta)
                .Subscribe(v => RectTransform.anchoredPosition = v)
                .AddTo(this);
        }

        protected override void OnStyleChanged(UIStyle style)
        {
            base.OnStyleChanged(style);

            if (BackdropImage != null)
            {
                Backdrop.Update(BackdropImage, DefaultBackdrop);
            }
        }

        public override void Show() => Show(null);

        public virtual void Show(IScreen screen)
        {
            base.Show();

            var targetScreen = screen ?? Context.FindScreen(ScreenNames.Windows);

            if (targetScreen != null)
            {
                Transform.SetParent(targetScreen.Transform);
            }

            RectTransform.anchoredPosition = Vector2.zero;
        }

        private void ShowBackdrop()
        {
            var index = Transform.GetSiblingIndex();
            var parent = new GameObject($"{Name} Backdrop", typeof(Image));

            parent.transform.SetParent(Transform.parent);
            parent.transform.SetSiblingIndex(index);

            _backdropImage = parent.GetComponent<Image>();

            Backdrop.Update(_backdropImage, DefaultBackdrop);

            var layout = parent.GetComponent<RectTransform>();

            layout.anchorMin = Vector2.zero;
            layout.anchorMax = Vector2.one;

            layout.offsetMin = Vector2.zero;
            layout.offsetMax = Vector2.zero;
        }

        private void HideBackdrop()
        {
            Destroy(_backdropImage.gameObject);

            _backdropImage = null;
        }

        protected override void OnDestroy()
        {
            _resizer?.Dispose();
            _resizer = null;

            base.OnDestroy();
        }

        protected override void ResetFromInstance(UIComponent component)
        {
            base.ResetFromInstance(component);

            var source = (Window) component;

            Movable = source.Movable;
            Resizable = source.Resizable;
            Modal = source.Modal;

            Backdrop = source.Backdrop;
        }

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static Window CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/Window");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<Window>();
        }
    }
}