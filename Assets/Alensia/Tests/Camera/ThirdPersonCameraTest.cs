using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Tests.Actor;
using NUnit.Framework;

namespace Alensia.Tests.Camera
{
    [TestFixture, Description("Test suite for ThirdPersonCamera class.")]
    public class ThirdPersonCameraTest : BaseOrbitingCameraTest<ThirdPersonCamera, IHumanoid>
    {
        protected override ThirdPersonCamera CreateCamera(UnityEngine.Camera camera)
        {
            var cam = new ThirdPersonCamera(camera);

            cam.RotationalConstraints.Up = 90;
            cam.RotationalConstraints.Down = 90;
            cam.RotationalConstraints.Side = 180;

            cam.WallAvoidanceSettings.AvoidWalls = false;

            cam.Initialize(Actor);

            return cam;
        }

        protected override IHumanoid CreateActor()
        {
            return new DummyHumanoid();
        }
    }
}