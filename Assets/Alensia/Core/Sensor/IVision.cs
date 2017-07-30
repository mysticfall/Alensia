using Alensia.Core.Geom;
using UnityEngine;

namespace Alensia.Core.Sensor
{
    public interface IVision : ISense, IDirectional
    {
        bool CanSee(Vector3 target);

        bool CanSee(Bounds bounds);

        void LookAt(Vector3 target);
    }
}