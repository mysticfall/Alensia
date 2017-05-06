using Alensia.Core.Camera;
using Alensia.Core.Input;
using Alensia.Core.Input.Generic;
using Alensia.Core.Locomotion;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Control
{
    public class WalkingControl : Vector2Control, ILocomotionControl<ILocomotion>, ICameraControl
    {
        public const string Id = "Locomotion.Move";

        public ILocomotion Locomotion { get; private set; }

        public ICameraManager CameraManager { get; private set; }

        public ViewSensitivity ViewSensitivity { get; private set; }

        public override IBindingKey<IAxisInput> X
        {
            get { return Keys.Horizontal; }
        }

        public override IBindingKey<IAxisInput> Y
        {
            get { return Keys.Vertical; }
        }

        public override bool Valid
        {
            get { return base.Valid && Locomotion.Active; }
        }

        public WalkingControl(
            ILocomotion locomotion,
            ICameraManager cameraManager,
            ViewSensitivity viewSensitivity,
            IInputManager inputManager) :
            base(inputManager)
        {
            Assert.IsNotNull(locomotion, "locomotion != null");
            Assert.IsNotNull(cameraManager, "cameraManager != null");
            Assert.IsNotNull(viewSensitivity, "viewSensitivity != null");

            Locomotion = locomotion;
            CameraManager = cameraManager;
            ViewSensitivity = viewSensitivity;
        }

        protected override void Execute(Vector2 input)
        {
            var camera = CameraManager.Mode as IRotatableCamera;

            if (input.magnitude > 0 && camera is IPerspectiveCamera)
            {
                var speed = Locomotion.RotateTowards(Vector3.up, camera.Heading);

                camera.Heading -= speed * Time.deltaTime;
            }

            var movement = input.normalized;

            Locomotion.Move(new Vector3(movement.x, 0, movement.y));
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