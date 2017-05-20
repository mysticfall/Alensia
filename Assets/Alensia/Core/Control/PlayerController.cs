using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Input;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class PlayerController : IPlayerController<IHumanoid>, IInitializable, IDisposable
    {
        public IList<IControl> Controls { get; }

        public IHumanoid Player { get; }

        public IInputManager InputManager { get; }

        public ICameraManager CameraManager { get; }

        public PlayerController(
            List<IControl> controls,
            IHumanoid player,
            IInputManager inputManager,
            ICameraManager cameraManager)
        {
            Assert.IsNotNull(controls, "controls != null");
            Assert.IsTrue(controls.Any(), "controls.Any()");

            Assert.IsNotNull(player, "player != null");
            Assert.IsNotNull(inputManager, "inputManager != null");
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            Controls = controls.AsReadOnly();
            Player = player;

            InputManager = inputManager;
            CameraManager = cameraManager;
        }

        public virtual void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //TODO Make it easier to register default key bindings.
            InputManager.Register(
                GameControl.Keys.ShowMenu,
                new TriggerDownInput(new KeyTrigger(KeyCode.Escape)));

            InputManager.Register(RotatableCameraControl.Keys.Yaw, new AxisInput("Mouse X"));
            InputManager.Register(RotatableCameraControl.Keys.Pitch, new AxisInput("Mouse Y"));
            InputManager.Register(OrbitingCameraControl.Keys.Zoom, new AxisInput("Mouse ScrollWheel", 0.15f));

            InputManager.Register(PlayerMovementControl.Keys.Horizontal, new AxisInput("Horizontal"));
            InputManager.Register(PlayerMovementControl.Keys.Vertical, new AxisInput("Vertical"));
            InputManager.Register(
                PlayerMovementControl.Keys.HoldToRun,
                new TriggerStateInput(new KeyTrigger(KeyCode.LeftShift)));

            foreach (var control in Controls)
            {
                control.Activate();
            }
        }

        public virtual void Dispose()
        {
            foreach (var control in Controls)
            {
                control.Deactivate();
            }
        }
    }
}