using System.Collections.Generic;
using Alensia.Core.Actor;
using Alensia.Core.Input;

namespace Alensia.Core.Control
{
    public interface IPlayerController<out T> where T : IActor
    {
        T Player { get; }

        IList<IControl> Controls { get; }

        IInputManager InputManager { get; }
    }
}