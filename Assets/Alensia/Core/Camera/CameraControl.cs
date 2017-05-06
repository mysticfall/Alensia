using Alensia.Core.Input;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public abstract class CameraControl : Control.Control, ICameraControl
    {
        public ICameraManager CameraManager { get; private set; }

        protected CameraControl(
            ICameraManager cameraManager, IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(cameraManager, "cameraManager != null");

            CameraManager = cameraManager;
        }
    }
}