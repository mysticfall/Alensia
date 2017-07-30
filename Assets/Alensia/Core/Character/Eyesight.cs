using System;
using Alensia.Core.Sensor;
using UnityEngine;

namespace Alensia.Core.Character
{
    public abstract class Eyesight : Sense, IBinocularVision
    {
        public abstract Transform LeftEye { get; }

        public abstract Transform RightEye { get; }

        public Vector3 Pivot => (LeftEye.position + RightEye.position) / 2;

        public Vector3 AxisForward => RightEye.forward;

        public Vector3 AxisUp => RightEye.up;

        public Vector3 AxisRight => RightEye.right;

        public virtual void LookAt(Vector3 target)
        {
            throw new NotImplementedException();
        }

        public virtual bool CanSee(Vector3 target)
        {
            throw new NotImplementedException();
        }

        public virtual bool CanSee(Bounds bounds)
        {
            throw new NotImplementedException();
        }
    }
}