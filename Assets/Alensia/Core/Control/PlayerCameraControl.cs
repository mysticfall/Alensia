using System;
using System.Collections.Generic;
using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Input;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class PlayerCameraControl : OrbitingCameraControl, IPlayerControl
    {
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

        public ViewSensitivity ViewSensitivity { get; }

        public override bool Valid => base.Valid &&
                                      Player != null &&
                                      CameraManager.Mode is IPerspectiveCamera;

        private IHumanoid _player;

        public PlayerCameraControl(
            [InjectOptional] IHumanoid player,
            ViewSensitivity viewSensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(cameraManager, inputManager)
        {
            Assert.IsNotNull(viewSensitivity, "viewSensitivity != null");

            Player = player;
            ViewSensitivity = viewSensitivity;
        }

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
            base.Subscribe(disposables);

            Scroll.OnChange
                .Where(_ => Valid)
                .Where(_ => CameraManager.Mode is IThirdPersonCamera)
                .Where(_ => CameraManager.Mode is IZoomableCamera)
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

        protected override void OnRotate(Vector2 input, IRotatableCamera camera)
        {
            var delta = new Vector2
            {
                x = input.x * ViewSensitivity.Horizontal,
                y = input.y * ViewSensitivity.Vertical
            };

            base.OnRotate(delta, camera);
        }

        protected override void OnZoom(float input, IZoomableCamera camera)
        {
            base.OnZoom(input * ViewSensitivity.Zoom, camera);
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