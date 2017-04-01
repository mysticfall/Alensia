using System;
using UnityEngine;

namespace Alensia.Core.Navigation
{
    [Serializable]
    public class NavigatorSettings
    {
        [Range(0, 100)]
        public float ForwardSpeed = 1;

        [Range(0, 100)]
        public float BackwardSpeed = 0.3f;

        [Range(0, 100)]
        public float SidewaysSpeed = 0.3f;

        [Range(0, 100)]
        public float UpwardSpeed;

        [Range(0, 100)]
        public float DownwardSpeed;

        [Range(0, 360)]
        public float AngularSpeed = 90;

        [Range(0, 180)]
        public float TurnBeforeMove = 90;
    }
}