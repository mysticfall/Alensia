using Alensia.Core.Actor;
using Alensia.Core.Geom;
using Alensia.Tests.Actor;
using NUnit.Framework;
using UnityEngine;
using TestRange = NUnit.Framework.RangeAttribute;

namespace Alensia.Tests.Camera
{
    [TestFixture, Description("Test suite for HeadMountedCamera class with a non humanoid actor.")]
    public class GenericHeadMountedCameraTest : HeadMountedCameraTest<IActor>
    {
        protected override IActor CreateActor()
        {
            return new DummyActor();
        }

        public override float ActualHeading =>
            GeometryUtils.NormalizeAspectAngle(Actor.Transform.localEulerAngles.y);

        public override float ActualElevation =>
            -GeometryUtils.NormalizeAspectAngle(Actor.Transform.localEulerAngles.x);

        [Test, Description("It should use the target actor itself as the pivot point.")]
        public void ShouldUseTargetActorItselfAsPivotPoint()
        {
            Camera.CameraOffset = new Vector3(0.5f, 0.2f, 0.1f);

            var offset = Actor.Transform.TransformDirection(Camera.CameraOffset) *
                         Camera.CameraOffset.magnitude;

            Expect(
                Camera.Pivot,
                Is.EqualTo(Actor.Transform.position + offset),
                "Unexpected pivot position."
            );
            Expect(
                Camera.AxisForward,
                Is.EqualTo(Actor.Transform.forward),
                "Unexpected pivot axis (forward).");
            Expect(
                Camera.AxisUp,
                Is.EqualTo(Actor.Transform.up),
                "Unexpected pivot axis (up).");
            Expect(
                Camera.Head,
                Is.EqualTo(Actor.Transform),
                "Unexpected head transform."
            );
        }
    }
}