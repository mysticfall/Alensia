using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public abstract class CameraMode : ICameraMode
    {
        public UnityEngine.Camera Camera { get; }

        public Transform Transform => Camera.transform;

        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active == value) return;

                if (value && !Valid)
                {
                    throw new InvalidOperationException("Invalid camera state.");
                }

                _active = value;

                if (_active)
                {
                    OnActivate();
                }
                else
                {
                    OnDeactivate();
                }
            }
        }

        public virtual bool Valid => true;

        private bool _active;

        protected CameraMode(UnityEngine.Camera camera)
        {
            Assert.IsNotNull(camera, "camera != null");

            Camera = camera;
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }
    }
}