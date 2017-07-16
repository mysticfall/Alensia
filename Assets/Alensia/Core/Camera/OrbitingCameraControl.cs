using System;
using System.Collections.Generic;
using Alensia.Core.Common;
using Alensia.Core.Control;
using Alensia.Core.Input;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public class OrbitingCameraControl : AggregateControl, ICameraControl
    {
        public ICameraManager CameraManager { get; }

        public ViewSensitivity Sensitivity { get; }

        public override bool Valid => _cameraSupported;

        private bool _cameraSupported;

        public OrbitingCameraControl(
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

        protected virtual bool Supports(ICameraMode camera) => camera is IOrbitingCamera;

        protected override IEnumerable<IControl> CreateChildren()
        {
            return new List<IControl>
            {
                new RotatableCameraControl(Sensitivity, CameraManager, InputManager),
                new ZoomableCameraControl(Sensitivity, CameraManager, InputManager)
            };
        }

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
        }
    }
}