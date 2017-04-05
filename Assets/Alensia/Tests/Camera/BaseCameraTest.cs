using Alensia.Core.Camera;
using NUnit.Framework;
using UnityEngine;

namespace Alensia.Tests.Camera
{
    public abstract class BaseCameraTest<T> : AssertionHelper where T : ICameraMode
    {
        public T Camera { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            var gameObject = new GameObject();
            var camera = gameObject.AddComponent<UnityEngine.Camera>();

            Camera = CreateCamera(camera);
            Camera.Activate();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (Camera != null)
            {
                Object.Destroy(Camera.Transform.gameObject);
            }

            Camera = default(T);
        }

        protected abstract T CreateCamera(UnityEngine.Camera camera);
    }
}