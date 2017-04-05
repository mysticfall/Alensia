using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Core.Common;
using Alensia.Tests.Actor;
using NUnit.Framework;
using UnityEngine;
using TestRange = NUnit.Framework.RangeAttribute;

namespace Alensia.Tests.Camera
{
    [TestFixture, Description("Test suite for HeadMountedCamera class.")]
    public class HeadMountedCameraTest : BaseCameraTest<HeadMountedCamera>
    {
        public const float Tolerance = 0.0001f;

        public IHumanoid Actor { get; private set; }

        public override void Setup()
        {
            Actor = new DummyActor();

            base.Setup();
        }

        public override void TearDown()
        {
            if (Actor != null)
            {
                Object.Destroy(Actor.Transform.gameObject);

                Actor = null;
            }

            base.TearDown();
        }

        protected override HeadMountedCamera CreateCamera(UnityEngine.Camera camera)
        {
            var cam = new HeadMountedCamera(camera);

            cam.RotationalConstraints.Up = 90;
            cam.RotationalConstraints.Side = 120;
            cam.RotationalConstraints.Down = 60;

            cam.Initialize(Actor);

            return cam;
        }

        public float ActualHeading
        {
            get
            {
                var head = Actor.GetBodyPart(HumanBodyBones.Head);
                var body = Actor.Transform;

                var direction = Vector3.ProjectOnPlane(head.forward, body.up);

                var heading = Vector3.Angle(body.forward, direction);
                var cross = body.InverseTransformDirection(Vector3.Cross(body.forward, direction));

                if (cross.y < 0) heading = -heading;

                return GeometryUtils.NormalizeAspectAngle(heading);
            }
        }

        public float ActualElevation
        {
            get
            {
                var head = Actor.GetBodyPart(HumanBodyBones.Head);
                var body = Actor.Transform;

                var direction = Quaternion.AngleAxis(-ActualHeading, body.up) * head.forward;

                var elevation = Vector3.Angle(body.forward, direction);
                var cross = body.InverseTransformDirection(Vector3.Cross(body.forward, direction));

                if (cross.x > 0) elevation = -elevation;

                return GeometryUtils.NormalizeAspectAngle(elevation);
            }
        }

        [Test, Description("It should use the target actor's head as the pivot point.")]
        public void ShouldUseHeadPartAsPivotPoint()
        {
            Expect(
                Camera.Pivot,
                Is.EqualTo(Actor.GetBodyPart(HumanBodyBones.Head))
            );
        }

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
    }
}