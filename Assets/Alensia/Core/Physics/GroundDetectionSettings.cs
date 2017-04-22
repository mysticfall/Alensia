using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Physics
{
    [Serializable]
    public class GroundDetectionSettings : IEditorSettings
    {
        public LayerMask GroundLayer = -1;

        public float Tolerance = 0.2f;
    }
}