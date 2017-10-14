using System;
using System.Collections.Generic;
using Alensia.Core.Camera;
using Alensia.Core.Character;
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

        public override CursorState CursorState => CursorState.Hidden;

        public override bool Valid => base.Valid && Player != null && CameraManager.Mode is IPerspectiveCamera;

        [InjectOptional] private IHumanoid _player;

        protected override bool Supports(ICameraMode mode) => mode is IFirstPersonCamera || mode is IThirdPersonCamera;

        protected override void Subscribe(ICollection<IDisposable> disposables)
        {
            base.Subscribe(disposables);

            Scroll.OnChange
                .Where(v => Valid && CameraManager.Mode is IThirdPersonCamera && v > 0)
                .Select(_ => (IZoomableCamera) CameraManager.Mode)
                .Where(c => Mathf.Approximately(c.Distance, c.DistanceSettings.Minimum))
                .Select(_ => (IThirdPersonCamera) CameraManager.Mode)
                .Subscribe(SwitchToFirstPerson)
                .AddTo(disposables);

            Scroll.OnChange
                .Where(v => Valid && CameraManager.Mode is IFirstPersonCamera && v < 0)
                .Select(_ => (IFirstPersonCamera) CameraManager.Mode)
                .Subscribe(SwitchToThirdPerson)
                .AddTo(disposables);
        }

        protected override void OnZoom(float input)
        {
            if (CameraManager.Mode is IZoomableCamera)
            {
                base.OnZoom(input);
            }
        }

        protected virtual void OnPlayerChange(IHumanoid player)
        {
            if (player == null) return;

            CameraManager.ToThirdPerson(player)?.ResetCamera();
        }

        protected void SwitchToFirstPerson(IThirdPersonCamera mode)
        {
            var firstPersonCamera = CameraManager.ToFirstPerson(Player);

            firstPersonCamera.Heading = mode.Heading;
            firstPersonCamera.Elevation = mode.Elevation;
        }

        protected void SwitchToThirdPerson(IFirstPersonCamera mode)
        {
            var thirdPersonCamera = CameraManager.ToThirdPerson(Player);

            thirdPersonCamera.Heading = mode.Heading;
            thirdPersonCamera.Elevation = mode.Elevation;
        }
    }
}