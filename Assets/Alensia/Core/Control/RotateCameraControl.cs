using Alensia.Core.Camera;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class RotateCameraControl : Vector2Control, ICameraControl
    {
        public const string Id = "Camera.Rotate";

        public override IBindingKey<IAxisInput> X
        {
            get { return Keys.Horizontal; }
        }

        public override IBindingKey<IAxisInput> Y
        {
            get { return Keys.Vertical; }
        }

        public ViewSensitivity ViewSensitivity { get; private set; }

        public ICameraManager CameraManager { get; private set; }

        public override bool Valid
        {
            get { return base.Valid && CameraManager.Mode is IRotatableCamera; }
        }

        public RotateCameraControl(
            ViewSensitivity viewSensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(viewSensitivity, "viewSensitivity != null");
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            ViewSensitivity = viewSensitivity;
            CameraManager = cameraManager;
        }

        protected override void Execute(Vector2 input)
        {
            Execute(input, CameraManager.Mode as IRotatableCamera);
        }

        protected virtual void Execute(Vector2 input, IRotatableCamera camera)
        {
            camera.Heading += input.x * ViewSensitivity.Horizontal;
            camera.Elevation += input.y * ViewSensitivity.Vertical;
        }

        public static class Keys
        {
            public static IBindingKey<IAxisInput> Horizontal =
                new BindingKey<IAxisInput>(Id + ".Horizontal");

            public static IBindingKey<IAxisInput> Vertical =
                new BindingKey<IAxisInput>(Id + ".Vertical");
        }
    }
}