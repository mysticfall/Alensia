using System;
using UnityEngine;

namespace Alensia.Core.UI
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
            Cursor.visible = Visible;
            Cursor.lockState = LockMode;

            Cursor.SetCursor(Image, Hotspot, CursorMode.Auto);
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