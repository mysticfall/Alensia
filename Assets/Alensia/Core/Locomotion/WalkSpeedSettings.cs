using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Locomotion
{
    [Serializable]
    public class WalkSpeedSettings : IEditorSettings
    {
        [Range(0, 100)]
        public float Forward = 1.4f;

        [Range(0, 100)]
        public float Backward = 1f;

        [Range(0, 100)]
        public float Sideway = 1f;

        [Range(0, 360)]
        public float Angular = 90;
    }
}