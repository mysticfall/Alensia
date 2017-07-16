using Alensia.Core.Common;
using Alensia.Core.Input;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public abstract class CameraControl : Control.Control, ICameraControl
    {
        public const string Category = "Camera";

        public ICameraManager CameraManager { get; }

        public ViewSensitivity Sensitivity { get; }

        public override bool Valid => base.Valid && _cameraSupported;

        private bool _cameraSupported;

        protected CameraControl(
            ViewSensitivity sensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(sensitivity, "sensitivity != null");
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            Sensitivity = sensitivity;
            CameraManager = cameraManager;

            CameraManager.OnCameraModeChange
                .Select(Supports)
                .Subscribe(v => _cameraSupported = v)
                .AddTo(this);
        }

        protected virtual bool Supports(ICameraMode camera) => true;
    }
}