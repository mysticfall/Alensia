using System;
using System.Collections.Generic;
using Alensia.Core.Common;
using Alensia.Core.Input;
using Alensia.Core.UI.Cursor;
using Zenject;
using IValidatable = Alensia.Core.Common.IValidatable;

namespace Alensia.Core.Control
{
    public interface IControl : IInitializable, IActivatable, IValidatable, IDisposable
    {
        IInputManager InputManager { get; }

        ICollection<IBindingKey> Bindings { get; }

        CursorState CursorState { get; }
    }
}