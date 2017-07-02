using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public abstract class CameraMode : BaseActivatable, ICameraMode
    {
        public UnityEngine.Camera Camera { get; }

        public Transform Transform => Camera.transform;

        public GameObject GameObject => Transform.gameObject;

        public virtual bool Valid => true;

        protected CameraMode(UnityEngine.Camera camera)
        {
            Assert.IsNotNull(camera, "camera != null");

            Camera = camera;
        }

        public virtual void Reset()
        {
        }
    }
}