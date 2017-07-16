using System;
using UnityEngine;

namespace Alensia.Core.Camera
{
    [Serializable]
    public class ViewSensitivity
    {
        [Range(0, 1)] public float Horizontal = 0.5f;

        [Range(0, 1)] public float Vertical = 0.5f;

        [Range(0, 1)] public float Zoom = 0.5f;
    }
}