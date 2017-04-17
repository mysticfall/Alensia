using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Camera
{
    [Serializable]
    public class DistanceSettings : IEditorSettings
    {
        [Tooltip("The initial distance between the camera and the target.")]
        [Range(0, 100)]
        public float Default = 1f;

        [Tooltip("The minimum distance between the camera and the target.")]
        [Range(0, 10)]
        public float Minimum = 0.3f;

        [Tooltip("The maximum distance of camera from the target.")]
        [Range(1, 100)]
        public float Maximum = 10f;
    }
}