using Alensia.Core.Camera;
using Alensia.Core.Common;
using Alensia.Core.UI;
using Alensia.Core.UI.Screen;
using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Entity
{
    public class EntityLabel : ComponentHandler<Panel>
    {
        [Inject]
        public ICameraManager CameraManager { get; }

        public IEntity Target { get; private set; }

        public IScreen Screen { get; private set; }

        public Label Label => _label;

        [PredefinedLiteral(typeof(ScreenNames))] public string TargetScreen = ScreenNames.Overlay;

        [SerializeField] private Label _label;

        public override void Initialize(IUIContext context)
        {
            base.Initialize(context);

            var screenManager = context as IRuntimeUIContext;

            if (screenManager == null) return;

            Screen = screenManager.FindScreen(TargetScreen);

            CameraManager.OnFocusChange
                .Subscribe(OnFocusChanged)
                .AddTo(this);

            CameraManager
                .OnCameraUpdate
                .Where(_ => Label != null && Screen != null)
                .Subscribe(_ => OnUpdate())
                .AddTo(this);

            Visible = false;
        }

        protected virtual void OnFocusChanged(Transform target)
        {
            Target = target?.GetComponentInChildren<IEntity>() ?? target?.GetComponentInParent<IEntity>();
        }

        protected virtual void OnUpdate()
        {
            if (Target != null)
            {
                var cam = CameraManager.Mode.Camera;

                RectTransform.anchoredPosition = Screen.ToViewportPoint(Target.Transform.position, cam);
                Label.Text = Target.DisplayName;
            }

            Visible = Target != null;
        }
    }
}