using System.Collections.Generic;
using Alensia.Core.Common;

namespace Alensia.Core.Control
{
    public interface IController : IActivatable, IDirectory<IControl>
    {
        IReadOnlyList<IControl> Controls { get; }

        void EnableControl(string name);

        void DisableControl(string name);
    }
}