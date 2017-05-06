using System;
using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class ZoomCameraControl : ScalarControl, ICameraControl, IActorControl<IHumanoid>
    {
        public const string Id = "Camera.Zoom";

        public override IBindingKey<IAxisInput> Axis
        {
            get { return Keys.Zoom; }
        }

        public IHumanoid Actor { get; private set; }

        public ViewSensitivity ViewSensitivity { get; private set; }

        public ICameraManager CameraManager { get; private set; }

        public ZoomCameraControl(
            IHumanoid actor,
            ViewSensitivity viewSensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(actor, "actor != null");
            Assert.IsNotNull(viewSensitivity, "viewSensitivity != null");
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            Actor = actor;
            ViewSensitivity = viewSensitivity;
            CameraManager = cameraManager;
        }

        protected override void Execute(float input)
        {
            var camera = CameraManager.Mode;

            var zoomableCamera = camera as IZoomableCamera;
            var minimumZoom = false;

            if (zoomableCamera != null)
            {
                minimumZoom = Mathf.Approximately(
                    zoomableCamera.Distance, zoomableCamera.DistanceSettings.Minimum);

                zoomableCamera.Distance -= Math.Sign(input) * ViewSensitivity.Zoom;
            }

            var firstPersonCamera = camera as IFirstPersonCamera;
            var thirdPersonCamera = camera as IThirdPersonCamera;

            if (firstPersonCamera != null && input < 0)
            {
                thirdPersonCamera = CameraManager.ToThirdPerson(Actor);

                thirdPersonCamera.Heading = firstPersonCamera.Heading;
                thirdPersonCamera.Elevation = firstPersonCamera.Elevation;
            }
            else if (thirdPersonCamera != null && minimumZoom && input > 0)
            {
                firstPersonCamera = CameraManager.ToFirstPerson(Actor);

                firstPersonCamera.Heading = thirdPersonCamera.Heading;
                firstPersonCamera.Elevation = thirdPersonCamera.Elevation;
            }
        }

        public static class Keys
        {
            public static IBindingKey<IAxisInput> Zoom = new BindingKey<IAxisInput>(Id + ".Zoom");
        }
    }
}