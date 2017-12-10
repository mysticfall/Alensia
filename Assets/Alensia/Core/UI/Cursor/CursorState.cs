using System;
using UnityEngine;
using UECursor = UnityEngine.Cursor;

namespace Alensia.Core.UI.Cursor
{
    [Serializable]
    public class CursorState
    {
        public CursorLockMode LockState => _lockState;

        public bool Visible => _visible;

        [SerializeField] private CursorLockMode _lockState;

        [SerializeField] private bool _visible;

        public CursorState() : this(CursorLockMode.Locked, false)
        {
        }

        public CursorState(CursorLockMode lockState, bool visible)
        {
            _lockState = lockState;
            _visible = visible;
        }

        public CursorState Lock(CursorLockMode lockState) => new CursorState(lockState, Visible);

        public CursorState Show(bool visible) => new CursorState(LockState, visible);

        public void Apply()
        {
            UECursor.lockState = LockState;
            UECursor.visible = Visible;
        }

        protected bool Equals(CursorState other) => _lockState == other._lockState && _visible == other._visible;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((CursorState) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) LockState * 397) ^ Visible.GetHashCode();
            }
        }

        public static CursorState Current => new CursorState(UECursor.lockState, UECursor.visible);

        public static readonly CursorState Vislbe = new CursorState(CursorLockMode.None, true);

        public static readonly CursorState Hidden = new CursorState(CursorLockMode.Locked, false);

        public static readonly CursorState Confined = new CursorState(CursorLockMode.Confined, true);
    }
}