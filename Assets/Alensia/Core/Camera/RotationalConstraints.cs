using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Camera
{
    [Serializable]
    public class RotationalConstraints : IEditorSettings
    {
        [Range(0, 90)]
        public float Up = 80f;

        [Range(0, 90)]
        public float Down = 80f;

        [Range(0, 180)]
        public float Side = 80f;
    }
}