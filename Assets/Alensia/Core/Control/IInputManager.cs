using UnityEngine;

namespace Alensia.Core.Control
{
    public interface IInputManager
    {
        ViewSensitivity Sensitivity { get; }

        Vector2 LastView { get; }

        Vector2 LastMovement { get; }

        float LastZoom { get; }
    }
}