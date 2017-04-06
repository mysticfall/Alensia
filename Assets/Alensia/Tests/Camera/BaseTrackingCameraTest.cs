using Alensia.Core.Actor;
using Alensia.Core.Camera;
using NUnit.Framework;
using UnityEngine;

namespace Alensia.Tests.Camera
{
    public abstract class BaseTrackingCameraTest<TCamera, TActor> : BaseCameraTest<TCamera>
        where TCamera : ITrackingCamera where TActor : IActor
    {
        public TActor Actor { get; private set; }

        public override void Setup()
        {
            Actor = CreateActor();

            base.Setup();
        }

        public override void TearDown()
        {
            if (Actor != null)
            {
                Object.Destroy(Actor.Transform.gameObject);

                Actor = default(TActor);
            }

            base.TearDown();
        }

        protected abstract TActor CreateActor();

        [Test, Description("Initialize() should initialize the camera with the given target.")]
        public void InitializeShouldSetTheTarget()
        {
            Expect(Actor, Is.EqualTo(Camera.Target), "Unexpected camera target.");
        }
    }
}