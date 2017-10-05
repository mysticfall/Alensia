using UniRx;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Camera
{
    public abstract class CameraControl : Control.Control, ICameraControl
    {
        public const string Category = "Camera";

        [Inject]
        public ICameraManager CameraManager { get; }

        public ViewSensitivity Sensitivity => _sensitivity;

        public override bool Valid => base.Valid && _cameraSupported;

        [SerializeField] private ViewSensitivity _sensitivity;

        private bool _cameraSupported;

        protected CameraControl()
        {
            _sensitivity = new ViewSensitivity();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            CameraManager.OnCameraModeChange
                .Select(Supports)
                .Subscribe(v => _cameraSupported = v)
                .AddTo(this);
        }

        protected virtual bool Supports(ICameraMode mode) => true;
    }
}