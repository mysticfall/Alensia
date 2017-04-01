using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Camera
{
    [Serializable]
    public class WallAvoidanceSettings : IEditorSettings
    {
        [Tooltip("Automatically change camera distance to prevent its view from being obstructed by walls.")]
        public bool AvoidWalls = true;

        [Tooltip("The closest distance the camera can be placed near a wall.")]
        [Range(0, 1)]
        public float MinimumDistance = 0.1f;
    }
}