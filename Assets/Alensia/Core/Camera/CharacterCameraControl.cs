using Alensia.Core.Input;

namespace Alensia.Core.Camera
{
    public class CharacterCameraControl : OrbitingCameraControl
    {
        public CharacterCameraControl(
            ViewSensitivity sensitivity,
            ICameraManager cameraManager,
            IInputManager inputManager) : base(sensitivity, cameraManager, inputManager)
        {
        }

        protected override bool Supports(ICameraMode camera) => camera is CharacterCamera;
    }
}