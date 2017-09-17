using System.Collections.Generic;
using System.Linq;
using Alensia.Core.UI.Resize;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Alensia.Core.UI
{
    public class Window : Panel, IWindow
    {
        public override IList<IComponent> Children =>
            base.Children.Where(c => c.Transform.parent == ContentPanel).ToList();

        public DraggableHeader Header =>
            _header ?? (_header = Transform.Find("Header").GetComponent<DraggableHeader>());

        public Transform ContentPanel => _content ?? (_content = Transform.Find("Content"));

        public Transform ButtonPanel => _buttons ?? (_buttons = Transform.Find("Buttons"));

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

        [SerializeField, HideInInspector] private DraggableHeader _header;

        [SerializeField, HideInInspector] private VerticalLayoutGroup _layoutGroup;

        private ResizeHelper _resizer;

        private Transform _content;

        private Transform _buttons;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            if (!Application.isPlaying) return;

            Header?.Initialize(Context);

            Header?.OnDrag
                .Select(e => RectTransform.anchoredPosition + e.delta)
                .Subscribe(v => RectTransform.anchoredPosition = v)
                .AddTo(this);

            if (ButtonPanel != null)
            {
                var buttons = ButtonPanel.GetComponents<IComponent>();

                foreach (var button in buttons)
                {
                    button.Initialize(context);
                }
            }

            _resizer = new ResizeHelper(this);

            _resizer.Initialize();
            _resizer.Activate();
        }

        protected override void OnDestroy()
        {
            _resizer?.Dispose();
            _resizer = null;

            base.OnDestroy();
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