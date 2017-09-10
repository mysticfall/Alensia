using UnityEngine;

namespace Alensia.Demo.UMA
{
    public class FocusTarget : MonoBehaviour
    {
        public HumanBodyBones Target;

        [Range(0f, 1f)]
        public float Zoom = 0.5f;
    }
}