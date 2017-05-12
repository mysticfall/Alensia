using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public abstract class CameraMode : ICameraMode
    {
        public UnityEngine.Camera Camera { get; }

        public Transform Transform => Camera.transform;

        public bool Active { get; private set; }

        public virtual bool Valid => true;

        protected CameraMode(UnityEngine.Camera camera)
        {
            Assert.IsNotNull(camera, "camera != null");

            Camera = camera;
        }

        public void Activate()
        {
            if (Valid)
            {
                if (Active) return;

                Active = true;

                OnActivate();
            }
            else
            {
                throw new InvalidOperationException("Invalid camera state.");
            }
        }

        public void Deactivate()
        {
            if (!Active) return;

            Active = false;

            OnDeactivate();
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }
    }
}