using System;
using System.Collections.Generic;
using Alensia.Core.Camera;
using Alensia.Core.Character;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using Alensia.Core.UI.Cursor;
using UniRx;
using UnityEngine;
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

        public IBindingKey<IAxisInput> Zoom => ZoomableCameraControl.Keys.Zoom;

        protected IAxisInput Scroll { get; private set; }

        public override CursorState CursorState => CursorState.Hidden;

        public override bool Valid => base.Valid && Player != null;

        private IHumanoid _player;

        public PlayerCameraControl(
            [InjectOptional] IHumanoid player,
            ViewSensitivity sensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(sensitivity, cameraManager, inputManager)
        {
            Player = player;
        }

        protected override bool Supports(ICameraMode camera) =>
            camera is IFirstPersonCamera || camera is IThirdPersonCamera;

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
            base.Subscribe(disposables);

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