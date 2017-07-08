using Alensia.Core.Character;
using Alensia.Core.Geom;
using Alensia.Tests.Character;
using NUnit.Framework;
using UnityEngine;
using TestRange = NUnit.Framework.RangeAttribute;

namespace Alensia.Tests.Camera
{
    [TestFixture, Description("Test suite for HeadMountedCamera class with a non humanoid character.")]
    public class GenericHeadMountedCameraTest : HeadMountedCameraTest<ICharacter>
    {
        protected override ICharacter CreateCharacter()
        {
            return new DummyCharacter();
        }

        public override float ActualHeading =>
            GeometryUtils.NormalizeAspectAngle(Character.Transform.localEulerAngles.y);

        public override float ActualElevation =>
            -GeometryUtils.NormalizeAspectAngle(Character.Transform.localEulerAngles.x);

        [Test, Description("It should use the target character itself as the pivot point.")]
        public void ShouldUseTargetActorItselfAsPivotPoint()
        {
            Camera.CameraOffset = new Vector3(0.5f, 0.2f, 0.1f);

            var offset = Character.Transform.TransformDirection(Camera.CameraOffset) *
                         Camera.CameraOffset.magnitude;

            Expect(
                Camera.Pivot,
                Is.EqualTo(Character.Transform.position + offset),
                "Unexpected pivot position."
            );
            Expect(
                Camera.AxisForward,
                Is.EqualTo(Character.Transform.forward),
                "Unexpected pivot axis (forward).");
            Expect(
                Camera.AxisUp,
                Is.EqualTo(Character.Transform.up),
                "Unexpected pivot axis (up).");
            Expect(
                Camera.Head,
                Is.EqualTo(Character.Transform),
                "Unexpected head transform."
            );
        }
    }
}