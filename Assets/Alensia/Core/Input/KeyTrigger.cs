using UnityEngine;
using static UnityEngine.Input;

namespace Alensia.Core.Input
{
    public class KeyTrigger : ITrigger
    {
        public readonly KeyCode Key;

        public bool Up => GetKeyUp(Key);

        public bool Down => GetKeyDown(Key);

        public bool Hold => GetKey(Key);

        public KeyTrigger(KeyCode key)
        {
            Key = key;
        }
    }
}