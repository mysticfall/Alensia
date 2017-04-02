using System.Collections.Generic;
using Alensia.Core.Actor;
using Alensia.Core.Camera;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class FirstAndThirdPersonController : IPlayerController<IHumanoid>, IInitializable, ITickable
    {
        public IHumanoid Player { get; private set; }

        public IInputManager InputManager { get; private set; }

        public ICameraManager CameraManager { get; private set; }

        public ViewSensitivity ViewSensitivity { get; private set; }

        private readonly List<ICameraControl> _controls;

        public FirstAndThirdPersonController(
            ViewSensitivity viewSensitivity,
            IHumanoid player,
            IInputManager inputManager,
            ICameraManager cameraManager)
        {
            Assert.IsNotNull(viewSensitivity, "viewSensitivity != null");
            Assert.IsNotNull(player, "player != null");
            Assert.IsNotNull(inputManager, "inputManager != null");
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            ViewSensitivity = viewSensitivity;

            Player = player;

            InputManager = inputManager;
            CameraManager = cameraManager;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _controls = new List<ICameraControl>
            {
                new SwitchToFirstPerson(this),
                new SwitchToThirdPerson(this),
                new ZoomCamera(this),
                new RotateCamera(this),
                new Turn(this),
                new Walk(this)
            };
        }

        public void Initialize()
        {
            CameraManager.ToThirdPerson(Player);
        }

        public void Tick()
        {
            var camera = CameraManager.Mode;

            _controls.ForEach(c => camera = c.Control(camera));
        }

        private interface ICameraControl
        {
            bool Supports(ICameraMode camera);

            ICameraMode Control(ICameraMode camera);
        }

        private abstract class CameraControl<T> : ICameraControl where T : class, ICameraMode
        {
            protected readonly FirstAndThirdPersonController Controller;

            protected CameraControl(FirstAndThirdPersonController controller)
            {
                Controller = controller;
            }

            public virtual bool Supports(ICameraMode camera)
            {
                return camera is T;
            }

            public ICameraMode Control(ICameraMode camera)
            {
                return Supports(camera) ? Process(camera as T) : camera;
            }

            protected abstract ICameraMode Process(T camera);
        }

        private class ZoomCamera : CameraControl<IZoomableCamera>
        {
            public ZoomCamera(FirstAndThirdPersonController controller) : base(controller)
            {
            }

            protected override ICameraMode Process(IZoomableCamera camera)
            {
                camera.Distance += Controller.InputManager.LastZoom;

                return camera;
            }
        }

        private class SwitchToFirstPerson : CameraControl<IZoomableCamera>
        {
            public SwitchToFirstPerson(FirstAndThirdPersonController controller) : base(controller)
            {
            }

            public override bool Supports(ICameraMode camera)
            {
                return base.Supports(camera) && camera is IThirdPersonCamera;
            }

            protected override ICameraMode Process(IZoomableCamera camera)
            {
                ICameraMode nextCamera = camera;

                if (Mathf.Approximately(camera.Distance, camera.DistanceSettings.Minimum) &&
                    Controller.InputManager.LastZoom < 0)
                {
                    nextCamera = Controller.CameraManager.ToFirstPerson(Controller.Player);
                }

                return nextCamera;
            }
        }

        private class SwitchToThirdPerson : CameraControl<IFirstPersonCamera>
        {
            public SwitchToThirdPerson(FirstAndThirdPersonController controller) : base(controller)
            {
            }

            protected override ICameraMode Process(IFirstPersonCamera camera)
            {
                ICameraMode nextCamera = camera;

                if (Controller.InputManager.LastZoom > 0)
                {
                    nextCamera = Controller.CameraManager.ToThirdPerson(Controller.Player);
                }

                return nextCamera;
            }
        }

        private class RotateCamera : CameraControl<IRotatableCamera>
        {
            public RotateCamera(FirstAndThirdPersonController controller) : base(controller)
            {
            }

            protected override ICameraMode Process(IRotatableCamera camera)
            {
                camera.Heading += Controller.InputManager.LastView.x;
                camera.Elevation += Controller.InputManager.LastView.y;

                return camera;
            }
        }

        private class Turn : CameraControl<IRotatableCamera>
        {
            public Turn(FirstAndThirdPersonController controller) : base(controller)
            {
            }

            public override bool Supports(ICameraMode camera)
            {
                return base.Supports(camera) && Controller.InputManager.LastMovement.magnitude > 0;
            }

            protected override ICameraMode Process(IRotatableCamera camera)
            {
                var player = Controller.Player;

                var prev = player.Transform.eulerAngles.y;

                player.Locomotion.Turn(camera.Heading);

                camera.Heading -= player.Transform.eulerAngles.y - prev;

                return camera;
            }
        }

        class Walk : CameraControl<ICameraMode>
        {
            public Walk(FirstAndThirdPersonController controller) : base(controller)
            {
            }

            protected override ICameraMode Process(ICameraMode camera)
            {
                var locomotion = Controller.Player.Locomotion;
                var movement = Controller.InputManager.LastMovement;

                locomotion.Walk(movement.normalized);

                return camera;
            }
        }
    }
}