using UnityEngine;

namespace Alensia.Core.Geom
{
    public interface IDirectional
    {
        Vector3 Pivot { get; }

        Vector3 AxisForward { get; }

        Vector3 AxisUp { get; }

        Vector3 AxisRight { get; }
    }
}