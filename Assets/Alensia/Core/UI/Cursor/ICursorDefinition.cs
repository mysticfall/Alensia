using System;
using UnityEngine;

namespace Alensia.Core.UI.Cursor
{
    public interface ICursorDefinition
    {
        string Name { get; }

        Vector2 Size { get; }

        Vector2 Hotspot { get; }

        IDisposable Apply();
    }
}