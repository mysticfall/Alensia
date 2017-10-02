using Alensia.Core.UI.Resize;
using UniRx;
using UnityEngine;

namespace Alensia.Core.UI
{
    public class ResizableWindow : Window
    {
        public bool Resizable
        {
            get { return _resizable.Value; }
            set { _resizable.Value = value; }
        }

        [SerializeField] private BoolReactiveProperty _resizable;

        private ResizeHelper _resizer;

        protected override void InitializeComponent(IUIContext context, bool isPlaying)
        {
            base.InitializeComponent(context, isPlaying);

            if (!isPlaying) return;

            _resizer = new ResizeHelper(this);

            _resizer.Initialize();
            _resizer.Activate();

            _resizable
                .Where(_ => _resizer != null)
                .Subscribe(v => _resizer.Active = v)
                .AddTo(this);
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

            var source = (ResizableWindow) component;

            Resizable = source.Resizable;
        }
    }
}