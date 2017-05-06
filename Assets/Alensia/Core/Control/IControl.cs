using System.Collections.Generic;
using Alensia.Core.Common;
using Alensia.Core.Input;

namespace Alensia.Core.Control
{
    public interface IControl : IActivatable, IValidatable
    {
        IInputManager InputManager { get; }

        ICollection<IBindingKey> Bindings { get; }
    }
}