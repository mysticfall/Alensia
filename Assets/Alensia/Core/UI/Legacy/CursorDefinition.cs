using System;
using UnityEngine;

namespace Alensia.Core.UI.Legacy
{
    [Serializable]
    public class CursorDefinition
    {
        public bool Visible;

        public CursorLockMode LockMode;

        public Vector2 Hotspot;

        public Texture2D Image;

        public void Apply()
        {
            UnityEngine.Cursor.visible = Visible;
            UnityEngine.Cursor.lockState = LockMode;

            UnityEngine.Cursor.SetCursor(Image, Hotspot, CursorMode.Auto);
        }

        public static CursorDefinition Hidden = new CursorDefinition
        {
            Visible = false,
            LockMode = CursorLockMode.Locked
        };

        public static CursorDefinition Default = new CursorDefinition
        {
            Visible = true,
            LockMode = CursorLockMode.None
        };
    }
}