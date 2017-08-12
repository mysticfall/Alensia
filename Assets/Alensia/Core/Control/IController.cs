using System.Collections.Generic;
using Alensia.Core.Common;
using Alensia.Core.UI.Cursor;

namespace Alensia.Core.Control
{
    public interface IController : IActivatable
    {
        CursorState DefaultCursorState { get; }

        IReadOnlyList<IControl> Controls { get; }

        T FindControl<T>() where T : IControl;

        IReadOnlyList<T> FindControls<T>() where T : IControl;

        void EnableControls<T>() where T : IControl;

        void DisableControls<T>() where T : IControl;
    }
}