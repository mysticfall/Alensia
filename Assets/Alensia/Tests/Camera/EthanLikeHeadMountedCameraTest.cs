using Alensia.Core.Actor;
using Alensia.Core.Camera;
using Alensia.Tests.Actor;
using NUnit.Framework;
using UnityEngine;

namespace Alensia.Tests.Camera
{
    [TestFixture,
     System.ComponentModel.Description("Test suite for HeadMountedCamera class with an Ethan-like character.")]
    public class EthanLikeHeadMountedCameraTest : HumanoidHeadMountedCameraTest
    {
        protected override IHumanoid CreateActor()
        {
            var humanoid = new DummyHumanoid
            {
                Head = {localEulerAngles = new Vector3(0, 90, -90)}
            };

            var mountPoint = new GameObject(HeadMountedCamera.MountPointName);

            mountPoint.transform.parent = humanoid.Head.transform;
            mountPoint.transform.localPosition = Vector3.zero;

            return humanoid;
        }

        [Test, Description("It should find an object named 'CameraMount' and use it as a pivot point.")]
        public void ShouldUseMountPointAsPivotIfAvailable()
        {
            Expect(
                Camera.Pivot.name,
                Is.EqualTo(HeadMountedCamera.MountPointName),
                "Unexpected pivot object.");
        }
    }
}