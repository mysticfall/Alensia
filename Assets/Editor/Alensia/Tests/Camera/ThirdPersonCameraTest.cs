using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Tests.Actor;
using NUnit.Framework;
using UnityEngine;

namespace Alensia.Tests.Camera
{
    [TestFixture, Description("Test suite for ThirdPersonCamera class.")]
    public class ThirdPersonCameraTest : OrbitingCameraTest<ThirdPersonCamera, IHumanoid>
    {
        private GameObject _obstacle;

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            if (_obstacle == null) return;

            Object.Destroy(_obstacle);

            _obstacle = null;
        }

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

        [Test, Description("It should adjust camera position according to obstacles when AvoidWalls is true.")]
        [TestCase(0, 0, 10, 1, 4)]
        [TestCase(0, 0, 2, 1, 2)]
        [TestCase(0, 0, 10, 2, 3)]
        [TestCase(45, 0, 10, 1, 10)]
        [TestCase(0, 45, 10, 1, 10)]
        public void ShouldAdjustCameraPositionAccordingToObstacles(
            float heading,
            float elevation,
            float distance,
            float proximity,
            float actual)
        {
            var transform = Actor.Transform;

            _obstacle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            _obstacle.transform.position = transform.position + new Vector3(0, 1, -5);

            Camera.WallAvoidanceSettings.AvoidWalls = true;
            Camera.WallAvoidanceSettings.MinimumDistance = proximity;

            Camera.Heading = heading;
            Camera.Elevation = elevation;
            Camera.Distance = distance;

            Expect(
                ActualDistance,
                Is.EqualTo(actual).Within(Tolerance),
                "Unexpected camera distance.");
        }
    }
}