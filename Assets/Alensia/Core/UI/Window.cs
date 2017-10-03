using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Alensia.Core.UI.Resize;
using Alensia.Core.UI.Screen;
using UniRx;
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

        public DraggableHeader Header =>
            _header ?? (_header = Transform.Find("Header").GetComponent<DraggableHeader>());

        public Transform ContentPanel => _content ?? (_content = Transform.Find("Content"));

        public override IList<IComponent> Children => ContentPanel.GetChildren<IComponent>().ToList();

        protected VerticalLayoutGroup LayoutGroup =>
            _layoutGroup ?? (_layoutGroup = GetComponent<VerticalLayoutGroup>());

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

        [SerializeField, HideInInspector] private DraggableHeader _header;

        [SerializeField, HideInInspector] private VerticalLayoutGroup _layoutGroup;

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