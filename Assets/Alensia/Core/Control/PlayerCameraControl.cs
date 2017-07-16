using System;
using System.Collections.Generic;
using Alensia.Core.Camera;
using Alensia.Core.Character;
using Alensia.Core.Common;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class PlayerCameraControl : AggregateControl, IPlayerControl, ICameraControl
    {
        public ICameraManager CameraManager { get; }

        public ViewSensitivity Sensitivity { get; }

        public IHumanoid Player
        {
            get { return _player; }
            set
            {
                lock (this)
                {
                    if (_player == value) return;

                    _player = value;
                }

                OnPlayerChange(value);
            }
        }

        public IBindingKey<IAxisInput> Zoom => ZoomableCameraControl.Keys.Zoom;

        protected IAxisInput Scroll { get; private set; }

        public override bool Valid => Player != null && _cameraSupported;

        private IHumanoid _player;

        private bool _cameraSupported;

        public PlayerCameraControl(
            [InjectOptional] IHumanoid player,
            ViewSensitivity sensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(sensitivity, "sensitivity != null");
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            Player = player;
            Sensitivity = sensitivity;
            CameraManager = cameraManager;

            CameraManager.OnCameraModeChange
                .Select(Supports)
                .Subscribe(v => _cameraSupported = v)
                .AddTo(this);
        }

        protected bool Supports(ICameraMode camera) =>
            camera is IFirstPersonCamera || camera is IThirdPersonCamera;

        protected override IEnumerable<IControl> CreateChildren()
        {
            return new List<IControl>
            {
                new RotatableCameraControl(Sensitivity, CameraManager, InputManager),
                new ZoomableCameraControl(Sensitivity, CameraManager, InputManager)
            };
        }

        protected override void OnBindingChange(IBindingKey key)
        {
            base.OnBindingChange(key);

            if (Equals(key, Zoom))
            {
                Scroll = InputManager.Get(Zoom);
            }
        }

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
            Scroll.OnChange
                .Where(_ => Valid)
                .Where(_ => CameraManager.Mode is IThirdPersonCamera)
                .Where(v => v > 0)
                .Select(_ => (IZoomableCamera) CameraManager.Mode)
                .Where(camera => Mathf.Approximately(camera.Distance, camera.DistanceSettings.Minimum))
                .Select(_ => (IThirdPersonCamera) CameraManager.Mode)
                .Subscribe(SwitchToFirstPerson)
                .AddTo(disposables);

            Scroll.OnChange
                .Where(_ => Valid)
                .Where(_ => CameraManager.Mode is IFirstPersonCamera)
                .Where(v => v < 0)
                .Select(_ => (IFirstPersonCamera) CameraManager.Mode)
                .Subscribe(SwitchToThirdPerson)
                .AddTo(disposables);
        }

        protected virtual void OnPlayerChange(IHumanoid player)
        {
            if (player == null) return;

            CameraManager.ToThirdPerson(player).Reset();
        }

        protected void SwitchToFirstPerson(IThirdPersonCamera camera)
        {
            var firstPersonCamera = CameraManager.ToFirstPerson(Player);

            firstPersonCamera.Heading = camera.Heading;
            firstPersonCamera.Elevation = camera.Elevation;
        }

        protected void SwitchToThirdPerson(IFirstPersonCamera camera)
        {
            var thirdPersonCamera = CameraManager.ToThirdPerson(Player);

            thirdPersonCamera.Heading = camera.Heading;
            thirdPersonCamera.Elevation = camera.Elevation;
        }
    }
}