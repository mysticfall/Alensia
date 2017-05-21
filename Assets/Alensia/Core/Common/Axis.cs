using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Common
{
    public enum Axis
    {
        X,
        Y,
        Z,
        InverseX,
        InverseY,
        InverseZ
    }

    public static class AxisExtensions
    {
        public static Vector3 Of(this Axis axis, Transform transform)
        {
            Assert.IsNotNull(transform, "transform != null");

            switch (axis)
            {
                case Axis.X:
                    return transform.right;
                case Axis.Y:
                    return transform.up;
                case Axis.Z:
                    return transform.forward;
                case Axis.InverseX:
                    return transform.right * -1;
                case Axis.InverseY:
                    return transform.up * -1;
                case Axis.InverseZ:
                    return transform.forward * -1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis));
            }
        }

        public static Vector3 Direction(this Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return Vector3.right;
                case Axis.Y:
                    return Vector3.up;
                case Axis.Z:
                    return Vector3.forward;
                case Axis.InverseX:
                    return Vector3.right * -1;
                case Axis.InverseY:
                    return Vector3.up * -1;
                case Axis.InverseZ:
                    return Vector3.forward * -1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis));
            }
        }
    }
}