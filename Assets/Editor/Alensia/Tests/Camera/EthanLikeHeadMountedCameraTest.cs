using Alensia.Core.Character;
using Alensia.Tests.Character;
using NUnit.Framework;
using UnityEngine;

namespace Alensia.Tests.Camera
{
    [TestFixture,
     System.ComponentModel.Description("Test suite for HeadMountedCamera class with an Ethan-like character.")]
    public class EthanLikeHeadMountedCameraTest : HumanoidHeadMountedCameraTest
    {
        protected override IHumanoid CreateCharacter()
        {
            var humanoid = new DummyHumanoid
            {
                Head = {localEulerAngles = new Vector3(0, 90, -90)}
            };

            return humanoid;
        }
    }
}