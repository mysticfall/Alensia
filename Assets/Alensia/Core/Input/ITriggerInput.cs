using System.Collections.Generic;
using Alensia.Core.Input.Generic;

namespace Alensia.Core.Input
{
    public interface ITriggerInput : IInput<float>
    {
        IList<ITrigger> Triggers { get; }
    }
}