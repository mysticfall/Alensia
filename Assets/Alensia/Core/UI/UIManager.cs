using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI
{
    public class UIManager : IUIManager
    {
        public UIManager()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        [Serializable]
        public class Settings : IEditorSettings
        {
        }
    }
}