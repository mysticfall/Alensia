using UnityEngine;

namespace Alensia.Core.Sensor
{
    public interface IBinocularVision : IVision
    {
        Transform LeftEye { get; }

        Transform RightEye { get; }
    }
}