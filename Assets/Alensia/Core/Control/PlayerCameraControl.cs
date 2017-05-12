using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Input;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class PlayerCameraControl<T> : OrbitingCameraControl where T : class, IActor
    {
        public T Player { get; }

        public ViewSensitivity ViewSensitivity { get; }

        public override bool Valid => base.Valid && CameraManager.Mode is IPerspectiveCamera;

        public PlayerCameraControl(
            T player,
            ViewSensitivity viewSensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(cameraManager, inputManager)
        {
            Assert.IsNotNull(player, "player != null");
            Assert.IsNotNull(viewSensitivity, "viewSensitivity != null");

            Player = player;
            ViewSensitivity = viewSensitivity;
        }

        public override void Initialize()
        {
            base.Initialize();

            CameraManager.ToThirdPerson(Player);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var toFirstPerson = Scroll.Value
                .Where(_ => CameraManager.Mode is IThirdPersonCamera)
                .Where(_ => CameraManager.Mode is IZoomableCamera)
                .Where(v => v > 0)
                .Select(_ => (IZoomableCamera) CameraManager.Mode)
                .Where(camera => Mathf.Approximately(camera.Distance, camera.DistanceSettings.Minimum))
                .Select(_ => (IThirdPersonCamera) CameraManager.Mode);

            var toThirdPerson = Scroll.Value
                .Where(_ => CameraManager.Mode is IFirstPersonCamera)
                .Where(v => v < 0)
                .Select(_ => (IFirstPersonCamera) CameraManager.Mode);

            Subsribe(toFirstPerson, SwitchToFirstPerson);
            Subsribe(toThirdPerson, SwitchToThirdPerson);
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