using Alensia.Core.Camera;
using Alensia.Core.Character;
using NUnit.Framework;
using UnityEngine;

namespace Alensia.Tests.Camera
{
    public abstract class TrackingCameraTest<TCamera, TCharacter> : CameraTest<TCamera>
        where TCamera : class, ITrackingCamera<ICharacter> where TCharacter : ICharacter
    {
        public TCharacter Character { get; private set; }

        protected override void PrepareScene()
        {
            Character = CreateCharacter();

            base.PrepareScene();
        }

        public override void TearDown()
        {
            if (Character != null)
            {
                Object.DestroyImmediate(Character.Transform.gameObject);

                Character = default(TCharacter);
            }

            base.TearDown();
        }

        protected abstract TCharacter CreateCharacter();

        [Test, Description("Initialize() should initialize the camera with the given target.")]
        public void InitializeShouldSetTheTarget()
        {
            Expect(Character, Is.EqualTo(Camera.Target), "Unexpected camera target.");
        }
    }
}