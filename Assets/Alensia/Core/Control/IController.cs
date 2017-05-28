using System.Collections.Generic;

namespace Alensia.Core.Control
{
    public interface IController
    {
        IList<IControl> Controls { get; }
    }
}