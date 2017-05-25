using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Geom;
using NUnit.Framework;
using UnityEngine;
using TestRange = NUnit.Framework.RangeAttribute;

namespace Alensia.Tests.Camera
{
    public abstract class HeadMountedCameraTest<T> :
        TrackingCameraTest<HeadMountedCamera, T> where T : IActor
    {
        protected override HeadMountedCamera CreateCamera(UnityEngine.Camera camera)
        {
            var cam = new HeadMountedCamera(camera);

            cam.RotationalConstraints.Up = 90;
            cam.RotationalConstraints.Side = 120;
            cam.RotationalConstraints.Down = 60;

            cam.Initialize(Actor);

            return cam;
        }

        public abstract float ActualHeading { get; }

        public abstract float ActualElevation { get; }

        [Test, Description("Changing Heading/Elevation should rotate the head to which the camera is attached.")]
        public void ShouldRotateHeadToProperPosition(
            [TestRange(-120, 120, 60)] float heading,
            [TestRange(-60, 80, 30)] float elevation)
        {
            Camera.Heading = heading;
            Camera.Elevation = elevation;

            var aspectAngle = GeometryUtils.NormalizeAspectAngle(heading);

            Expect(
                ActualHeading,
                Is.EqualTo(aspectAngle).Within(Tolerance),
                "Unexpected head heading.");
            Expect(
                ActualElevation,
                Is.EqualTo(elevation).Within(Tolerance),
                "Unexpected head elevation.");
        }

        [Test, Description("It should adjust the camera position according to the actor's position and rotation.")]
        public void ShouldReflectActorPositionAndRotation(
            [Values(-120, 60)] float heading,
            [Values(-40, 60)] float elevation)
        {
            Actor.Transform.eulerAngles = new Vector3
            {
                x = Random.Range(-180, 180),
                y = Random.Range(-180, 180),
                z = Random.Range(-180, 180)
            };

            Actor.Transform.position = new Vector3
            {
                x = Random.Range(-10, 10),
                y = Random.Range(-10, 10),
                z = Random.Range(-10, 10)
            };

            Camera.Heading = heading;
            Camera.Elevation = elevation;

            Expect(
                ActualHeading,
                Is.EqualTo(heading).Within(Tolerance),
                "Unexpected camera heading.");
            Expect(
                ActualElevation,
                Is.EqualTo(elevation).Within(Tolerance),
                "Unexpected camera elevation.");
        }

        [Test, Description("The camera should follow the actor's head position and rotation per every tick.")]
        public void ShouldFollowHeadPositionAndRotationPerEveryTick(
            [Values(-120, 60)] float heading,
            [Values(-40, 15)] float elevation)
        {
            Camera.Heading = heading;
            Camera.Elevation = elevation;

            Actor.Transform.eulerAngles = new Vector3
            {
                x = Random.Range(-180, 180),
                y = Random.Range(-180, 180),
                z = Random.Range(-180, 180)
            };

            Actor.Transform.position = new Vector3
            {
                x = Random.Range(-10, 10),
                y = Random.Range(-10, 10),
                z = Random.Range(-10, 10)
            };

            Camera.LateTick();

            Expect(
                ActualHeading,
                Is.EqualTo(heading).Within(Tolerance),
                "Unexpected camera heading.");
            Expect(
                ActualElevation,
                Is.EqualTo(elevation).Within(Tolerance),
                "Unexpected camera elevation.");
        }

        [Test, Description("It should return camera's rotation as heading when elevation is -90/90 degrees.")]
        public void ShouldUseCameraRotationAsHeadingWhenElevationIs90Degrees(
            [TestRange(-120, 120, 45)] float heading,
            [Values(-90, 90)] float elevation)
        {
            Actor.Transform.eulerAngles = new Vector3
            {
                x = Random.Range(-180, 180),
                y = Random.Range(-180, 180),
                z = Random.Range(-180, 180)
            };

            Camera.RotationalConstraints.Up = 90;
            Camera.RotationalConstraints.Down = 90;

            Camera.Heading = heading;
            Camera.Elevation = elevation;

            var expected = GeometryUtils.NormalizeAspectAngle(heading);

            Expect(
                ActualHeading,
                Is.EqualTo(expected).Within(Tolerance),
                "Unexpected camera heading.");
        }

        [Test, Description("Heading property should be clamped between the min. and the max. values.")]
        public void ShouldClampHeadingProperty()
        {
            Camera.RotationalConstraints.Side = 60;

            Camera.Heading = -80;

            Expect(
                Camera.Heading,
                Is.EqualTo(-60).Within(Tolerance),
                "Unexpected camera heading.");

            Camera.Heading = 80;

            Expect(
                Camera.Heading,
                Is.EqualTo(60).Within(Tolerance),
                "Unexpected camera heading.");
        }

        [Test, Description("Elevation property should be clamped between the min. and the max. values.")]
        public void ShouldClampElevationProperty()
        {
            Camera.RotationalConstraints.Up = 60;
            Camera.RotationalConstraints.Down = 30;

            Camera.Elevation = 80;

            Expect(
                Camera.Elevation,
                Is.EqualTo(60).Within(Tolerance),
                "Unexpected camera elevation.");

            Camera.Elevation = -50;

            Expect(
                Camera.Elevation,
                Is.EqualTo(-30).Within(Tolerance),
                "Unexpected camera elevation.");
        }

        [Test, Description("It should restore the head to its initial rotation when it is deactivated.")]
        public void ShouldRestoreHeadRotationUponDeactivation()
        {
            var rotation = Camera.Head.rotation;

            Camera.Heading = 30;
            Camera.Elevation = 80;

            Camera.Active = false;

            Expect(
                Camera.Head.rotation,
                Is.EqualTo(rotation),
                "Unexpected head rotation.");
        }
    }
}