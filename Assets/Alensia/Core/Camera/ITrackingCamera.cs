using UnityEngine;

namespace Alensia.Core.Camera
{
    public interface ITrackingCamera : ICameraMode
    {
        Transform Focus { get; set; }
    }
}