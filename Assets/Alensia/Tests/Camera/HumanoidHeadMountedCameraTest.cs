using Alensia.Core.Actor;
using Alensia.Core.Common;
using Alensia.Tests.Actor;
using NUnit.Framework;
using UnityEngine;
using TestRange = NUnit.Framework.RangeAttribute;

namespace Alensia.Tests.Camera
{
    [TestFixture, Description("Test suite for HeadMountedCamera class with a humanoid.")]
    public class HumanoidHeadMountedCameraTest : BaseHeadMountedCameraTest<IHumanoid>
    {
        public override float ActualHeading
        {
            get
            {
                var head = Camera.Pivot;
                var body = Camera.Target.Transform;

                var direction = Vector3.ProjectOnPlane(head.forward, body.up);

                var heading = Vector3.Angle(body.forward, direction);
                var cross = body.InverseTransformDirection(Vector3.Cross(body.forward, direction));

                if (cross.y < 0) heading = -heading;

                return GeometryUtils.NormalizeAspectAngle(heading);
            }
        }

        public override float ActualElevation
        {
            get
            {
                var head = Camera.Pivot;
                var body = Camera.Target.Transform;

                var direction = Quaternion.AngleAxis(-ActualHeading, body.up) * head.forward;

                var elevation = Vector3.Angle(body.forward, direction);
                var cross = body.InverseTransformDirection(Vector3.Cross(body.forward, direction));

                if (cross.x > 0) elevation = -elevation;

                return GeometryUtils.NormalizeAspectAngle(elevation);
            }
        }

        protected override IHumanoid CreateActor()
        {
            return new DummyHumanoid();
        }

        [Test, Description("It should use the target actor's head as the pivot point.")]
        public void ShouldUseHeadPartAsPivotPoint()
        {
            Expect(
                Camera.Pivot,
                Is.EqualTo(Actor.GetBodyPart(HumanBodyBones.Head)),
                "Unexpected pivot object."
            );
            Expect(
                Camera.Head,
                Is.EqualTo(Actor.GetBodyPart(HumanBodyBones.Head)),
                "Unexpected head transform."
            );
        }
    }
}