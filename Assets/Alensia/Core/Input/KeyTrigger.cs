using UnityEngine;

namespace Alensia.Core.Input
{
    public class KeyTrigger : ITrigger
    {
        public readonly KeyCode Key;

        public bool Up
        {
            get { return UnityEngine.Input.GetKeyUp(Key); }
        }

        public bool Down
        {
            get { return UnityEngine.Input.GetKeyDown(Key); }
        }

        public bool Hold
        {
            get { return UnityEngine.Input.GetKey(Key); }
        }

        public KeyTrigger(KeyCode key)
        {
            Key = key;
        }
    }
}