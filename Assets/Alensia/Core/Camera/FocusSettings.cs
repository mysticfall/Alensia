using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Camera
{
    [Serializable]
    public class FocusSettings : IEditorSettings
    {
        public bool TrackFocus = true;

        public float MaximumDistance = 2;

        public LayerMask Layer = -1;
    }
}