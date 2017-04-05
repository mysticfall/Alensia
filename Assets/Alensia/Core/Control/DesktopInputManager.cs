using System;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Control
{
    public class DesktopInputManager : IInputManager, ITickable
    {
        public ViewSensitivity Sensitivity { get; private set; }

        public Vector2 LastView { get; private set; }

        public Vector2 LastMovement { get; private set; }

        public float LastZoom { get; private set; }

        public DesktopInputManager() : this(new ViewSensitivity())
        {
        }

        [Inject]
        public DesktopInputManager(ViewSensitivity sensitivity)
        {
            Assert.IsNotNull(sensitivity, "sensitivity != null");

            Sensitivity = sensitivity;
        }

        public void Tick()
        {
            var xInput = Input.GetAxis("Mouse X") * Sensitivity.Horizontal;
            var yInput = Input.GetAxis("Mouse Y") * Sensitivity.Vertical;

            var hInput = Input.GetAxis("Horizontal");
            var vInput = Input.GetAxis("Vertical");

            LastView = new Vector2(xInput, yInput);
            LastMovement = new Vector2(hInput, vInput);

            LastZoom = -Math.Sign(Input.GetAxis("Mouse ScrollWheel")) * Sensitivity.Zoom;

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}